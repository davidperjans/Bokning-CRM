using FluentValidation;

namespace Application.Services.Commands.UpdateService
{
    public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DurationMinutes).InclusiveBetween(1, 480);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}