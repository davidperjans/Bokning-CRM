using Application.Bookings.Dtos;
using Application.Common;
using MediatR;

namespace Application.Bookings.Queries.GetMyBookings
{
    public sealed record GetMyBookingsQuery
        : IRequest<OperationResult<List<BookingDto>>>;
}
