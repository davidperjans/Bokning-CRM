using FluentValidation;

namespace Application.Admin.Commands.CreateStaff
{
    public sealed class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
    {
        public CreateStaffCommandValidator()
        {
            RuleFor(x => x.BusinessId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));

            RuleFor(x => x.Bio)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Bio));

            RuleFor(x => x.QualifiedServiceIds)
                .NotNull();

            RuleFor(x => x.QualifiedServiceIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("QualifiedServiceIds must contain unique values.");
        }
    }
}
