using Application.Bookings.Dtos;
using Application.Common;
using MediatR;

namespace Application.Bookings.Commands.CreateBooking
{
    public sealed record CreateBookingCommand(
        Guid BookingTypeId,
        Guid ResourceId,
        DateTime StartTimeUtc,
        string? Notes
    ) : IRequest<OperationResult<BookingDto>>;
}
