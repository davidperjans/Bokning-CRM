using FluentValidation;

namespace Application.Admin.Commands.UpdateBookingStatus
{
    public sealed class UpdateBookingStatusCommandValidator
        : AbstractValidator<UpdateBookingStatusCommand>
    {
        public UpdateBookingStatusCommandValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("Boknings-id saknas.");

            RuleFor(x => x.NewStatus)
                .IsInEnum()
                .WithMessage("Ogiltig bokningsstatus.");
        }
    }
}
