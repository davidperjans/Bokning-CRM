using MediatR;
using Application.Common;

namespace Application.Bookings.Commands.CancelBooking
{
    public sealed record CancelBookingCommand(
        Guid BookingId,
        string? CancellationReason
    ) : IRequest<OperationResult<bool>>;
}
