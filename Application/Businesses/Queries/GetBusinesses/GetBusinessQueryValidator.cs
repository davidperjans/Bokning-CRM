using FluentValidation;

namespace Application.Businesses.Queries.GetBusinesses
{
    public sealed class GetBusinessesQueryValidator : AbstractValidator<GetBusinessesQuery>
    {
        public GetBusinessesQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

            RuleFor(x => x.City).MaximumLength(100);
            RuleFor(x => x.Category).MaximumLength(100);
            RuleFor(x => x.Query).MaximumLength(200);

            RuleFor(x => x.Query)
                .Must(q => q is null || q.Trim().Length > 0)
                .WithMessage("query cannot be empty whitespace");
        }
    }
}
