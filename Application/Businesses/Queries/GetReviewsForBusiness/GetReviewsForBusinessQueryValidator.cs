using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.Queries.GetReviewsForBusiness
{
    public sealed class GetReviewsForBusinessQueryValidator : AbstractValidator<GetReviewsForBusinessQuery>
    {
        public GetReviewsForBusinessQueryValidator()
        {
            RuleFor(x => x.Slug)
                .NotEmpty()
                .MaximumLength(200)
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$") // typ "pizza-palace"
                .WithMessage("Invalid slug format.");
        }
    }
}
