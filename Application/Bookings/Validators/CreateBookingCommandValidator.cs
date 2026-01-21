using Application.Bookings.Commands.CreateBooking;
using FluentValidation;

namespace Application.Bookings.Validators
{
    public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.BookingTypeId)
                .NotEmpty()
                .WithMessage("Bokningstyp måste anges.");

            RuleFor(x => x.ResourceId)
                .NotEmpty()
                .WithMessage("Resurs måste anges.");

            RuleFor(x => x.StartTimeUtc)
                .NotEmpty()
                .WithMessage("Starttid måste anges.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Anteckningar får vara max 500 tecken.");
        }
    }
}
