using Application.Admin.DTOs;
using Application.Common;
using Application.Interface;
using Domain.Enum;
using MediatR;

namespace Application.Admin.Queries
{
    public sealed class GetAdminDashboardStatsHandler
        : IRequestHandler<GetAdminDashboardStatsQuery, OperationResult<AdminDashboardStatsDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public GetAdminDashboardStatsHandler(
            IBookingRepository bookingRepository,
            IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<AdminDashboardStatsDto>> Handle(
            GetAdminDashboardStatsQuery request,
            CancellationToken ct)
        {
            if (_userContext.BusinessId is null)
                return OperationResult<AdminDashboardStatsDto>.Failure("Business saknas i användarkontexten.");

            var businessId = _userContext.BusinessId.Value;

            var nowUtc = DateTime.UtcNow;
            var todayStartUtc = new DateTime(nowUtc.Year, nowUtc.Month, nowUtc.Day, 0, 0, 0, DateTimeKind.Utc);
            var todayEndUtc = todayStartUtc.AddDays(1);

            // OBS: Dessa metoder behöver finnas i IBookingRepository.
            var total = await _bookingRepository.CountForBusinessAsync(businessId, ct);
            var upcoming = await _bookingRepository.CountUpcomingForBusinessAsync(businessId, nowUtc, ct);
            var cancelled = await _bookingRepository.CountByStatusForBusinessAsync(businessId, BookingStatus.Cancelled, ct);
            var today = await _bookingRepository.CountInRangeForBusinessAsync(businessId, todayStartUtc, todayEndUtc, ct);

            var dto = new AdminDashboardStatsDto
            {
                TotalBookings = total,
                UpcomingBookings = upcoming,
                CancelledBookings = cancelled,
                BookingsToday = today
            };

            return OperationResult<AdminDashboardStatsDto>.Success(dto);
        }
    }
}
