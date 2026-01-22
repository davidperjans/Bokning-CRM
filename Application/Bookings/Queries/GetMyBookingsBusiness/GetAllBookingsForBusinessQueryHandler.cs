using Application.Bookings.Dtos;
using Application.Common;
using Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bookings.Queries.GetMyBookingsBusiness
{
    public sealed class GetAllBookingsForBusinessHandler
        : IRequestHandler<GetAllBookingsForBusinessQuery, OperationResult<List<BookingDto>>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public GetAllBookingsForBusinessHandler(
            IBookingRepository bookingRepository,
            IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<List<BookingDto>>> Handle(
            GetAllBookingsForBusinessQuery request,
            CancellationToken ct)
        {
            if (_userContext.BusinessId is null)
                return OperationResult<List<BookingDto>>.Failure("Business saknas i användarkontexten.");

            // (Här kan du senare lägga admin-check via role)
            var bookings = await _bookingRepository.GetForBusinessAsync(
                _userContext.BusinessId.Value,
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
