using Application.Bookings.Dtos;
using Application.Common;
using MediatR;

namespace Application.Bookings.Queries.GetMyBookingsBusiness
{
    public sealed record GetAllBookingsForBusinessQuery
        : IRequest<OperationResult<List<BookingDto>>>;
}
