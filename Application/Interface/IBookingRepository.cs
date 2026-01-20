using Domain.Models;

namespace Application.Interface
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken ct);
        Task AddAsync(Booking booking, CancellationToken ct);
        Task<bool> ExistsOverlappingAsync(
            Guid busniessId,
            Guid resourceId,
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken ct);

        Task<int> CountActiveForUserAsync(
        Guid businessId,
        Guid userId,
        CancellationToken ct);


    }
}
