using Application.Common;
using Domain.Enum;
using MediatR;

namespace Application.Admin.Commands.UpdateBookingStatus
{
    public sealed record UpdateBookingStatusCommand(
        Guid BookingId,
        BookingStatus NewStatus
    ) : IRequest<OperationResult<bool>>;
}
