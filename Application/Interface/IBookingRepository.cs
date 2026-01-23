using Domain.Enum;
using Domain.Models;

namespace Application.Interface
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken ct);
        Task AddAsync(Booking booking, CancellationToken ct);

        Task<bool> ExistsOverlappingAsync(
            Guid businessId,
            Guid resourceId,
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken ct);

        Task<Booking?> GetByIdForBusinessAsync(Guid businessId, Guid bookingId, CancellationToken ct);
        Task<int> CountActiveForUserAsync(Guid businessId, Guid userId, CancellationToken ct);
        Task<List<Booking>> GetForUserAsync(Guid businessId, Guid userId, CancellationToken ct);
        Task<List<Booking>> GetForBusinessAsync(Guid businessId, CancellationToken ct);
        Task<int> CountForBusinessAsync(Guid businessId, CancellationToken ct);
        Task<int> CountUpcomingForBusinessAsync(Guid businessId, DateTime nowUtc, CancellationToken ct);
        Task<int> CountByStatusForBusinessAsync(Guid businessId, BookingStatus status, CancellationToken ct);
        Task<int> CountInRangeForBusinessAsync(
            Guid businessId,
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken ct);
        Task<bool> UpdateStatusAsync(
            Guid businessId,
            Guid bookingId,
            BookingStatus newStatus,
            CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);


    }
}
