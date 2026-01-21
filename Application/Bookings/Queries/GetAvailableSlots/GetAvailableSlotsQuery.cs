using MediatR;
using Application.Common;
using Application.Bookings.DTOs;

namespace Application.Bookings.Queries.GetAvailableSlots
{
    public sealed record GetAvailableSlotsQuery(
        Guid BookingTypeId,
        DateTime StartDateUtc,
        DateTime EndDateUtc,
        Guid? ResourceId
    ) : IRequest<OperationResult<List<AvailableSlotDto>>>;
}
