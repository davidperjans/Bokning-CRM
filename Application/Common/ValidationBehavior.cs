using MediatR;
using FluentValidation;
using Application.Common;
using System.Reflection;

namespace Application.Common
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class // Här kan man begränsa till Result-typer
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorMessages = failures.Select(f => f.ErrorMessage).ToList();
                var resultType = typeof(TResponse);

                // Kontrollera om TResponse är av typen Result<T>
                if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(OperationResult<>))
                {
                    var failureMethod = resultType.GetMethod("ValidationFailure", BindingFlags.Public | BindingFlags.Static);

                    if (failureMethod != null)
                    {
                        // Returnera vårt Result-objekt med felmeddelanden
                        return (failureMethod.Invoke(null, new object[] { errorMessages }) as TResponse)!;
                    }
                }

                // Om vi inte använder Result<T>, kasta ett exception så att handlern inte körs
                throw new ValidationException(failures);
            }

            // Om inga valideringsfel finns, fortsätt till nästa steg (Handlern)
            return await next();
        }

    }
}