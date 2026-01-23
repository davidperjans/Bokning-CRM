using FluentValidation;

namespace Application.Services.Commands.CreateService
{
    public class CreateServiceValidator : AbstractValidator<CreateServiceCommand>
    {

        public CreateServiceValidator()
        {
            RuleFor(x => x.BusinessId)
                .NotEmpty().WithMessage("BusinessId is required.");
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes.")
                .LessThanOrEqualTo(480).WithMessage("Duration cannot exceed 480 minutes (8 hours).");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value.");
        }
        
    }
}