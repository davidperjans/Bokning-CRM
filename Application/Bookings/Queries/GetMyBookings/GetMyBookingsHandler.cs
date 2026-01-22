using Application.Bookings.Dtos;
using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Bookings.Queries.GetMyBookings
{
    public sealed class GetMyBookingsHandler
        : IRequestHandler<GetMyBookingsQuery, OperationResult<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public GetMyBookingsHandler(
            IBookingRepository bookingRepository,
            IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<List<BookingDto>>> Handle(
            GetMyBookingsQuery request,
            CancellationToken ct)
        {
            if (_userContext.UserId is null)
                return OperationResult<List<BookingDto>>.Failure("Du måste vara inloggad.");

            if (_userContext.BusinessId is null)
                return OperationResult<List<BookingDto>>.Failure("Business saknas i användarkontexten.");

            var bookings = await _bookingRepository.GetForUserAsync(
                _userContext.BusinessId.Value,
                _userContext.UserId.Value,
                ct);

            var dto = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                BusinessId = b.BusinessId,
                UserId = b.UserId,
                BookingTypeId = b.BookingTypeId,
                ResourceId = b.ResourceId,
                StartTimeUtc = b.StartTimeUtc,
                EndTimeUtc = b.EndTimeUtc,
                Status = b.Status,
                Notes = b.Notes
            }).ToList();

            return OperationResult<List<BookingDto>>.Success(dto);
        }
    }
}
