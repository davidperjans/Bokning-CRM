using Application.Bookings.Queries.GetAvailableSlots;
using FluentValidation;

namespace Application.Bookings.Validators
{
    public sealed class GetAvailableSlotsQueryValidator : AbstractValidator<GetAvailableSlotsQuery>
    {
        public GetAvailableSlotsQueryValidator()
        {
            RuleFor(x => x.BookingTypeId)
                .NotEmpty()
                .WithMessage("Bokningstyp måste anges.");

            RuleFor(x => x.StartDateUtc)
                .NotEmpty()
                .WithMessage("Startdatum måste anges.");

            RuleFor(x => x.EndDateUtc)
                .NotEmpty()
                .WithMessage("Slutdatum måste anges.");

            RuleFor(x => x)
                .Must(x => x.EndDateUtc >= x.StartDateUtc)
                .WithMessage("Slutdatum måste vara samma eller efter startdatum.");

            RuleFor(x => x.ResourceId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("Resurs-id får inte vara ett tomt GUID.");
        }
    }
}
