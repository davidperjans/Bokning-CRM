using Application.Interface;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken ct)
        {
            // Hämtar en bokning via Id (null om den inte finns)
            return await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bookingId, ct);
        }

        public async Task AddAsync(Booking booking, CancellationToken ct)
        {
            // Lägger till bokning i change tracker (glöm inte SaveChangesAsync i service/handler)
            await _context.Bookings.AddAsync(booking, ct);
        }

        public async Task<bool> ExistsOverlappingAsync(
            Guid businessId,
            Guid resourceId,
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken ct)
        {
            // Overlap-regel:
            // En befintlig bokning krockar om:
            // existing.Start < new.End  AND existing.End > new.Start
            return await _context.Bookings
                .AsNoTracking()
                .AnyAsync(b =>
                    b.BusinessId == businessId &&
                    b.ResourceId == resourceId &&
                    b.Status != BookingStatus.Cancelled &&
                    b.StartTimeUtc < endUtc &&
                    b.EndTimeUtc > startUtc,
                    ct);
        }

        public async Task<int> CountActiveForUserAsync(Guid businessId, Guid userId, CancellationToken ct)
        {
            // "Aktiva" = allt som inte är Cancelled
            return await _context.Bookings
                .AsNoTracking()
                .CountAsync(b =>
                    b.BusinessId == businessId &&
                    b.UserId == userId &&
                    b.Status != BookingStatus.Cancelled,
                    ct);
        }

        public async Task<List<Booking>> GetForUserAsync(
            Guid businessId,
            Guid userId,
            CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(b =>
                    b.BusinessId == businessId &&
                    b.UserId == userId)
                .OrderBy(b => b.StartTimeUtc)
                .ToListAsync(ct);
        }

        public async Task<List<Booking>> GetForBusinessAsync(
            Guid businessId,
            CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(b => b.BusinessId == businessId)
                .OrderBy(b => b.StartTimeUtc)
                .ToListAsync(ct);
        }

        public async Task<Booking?> GetByIdForBusinessAsync(Guid businessId, Guid bookingId, CancellationToken ct)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.BusinessId == businessId && b.Id == bookingId, ct);
        }

        public async Task<int> CountForBusinessAsync(Guid businessId, CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .CountAsync(b => b.BusinessId == businessId, ct);
        }

        public async Task<int> CountUpcomingForBusinessAsync(Guid businessId, DateTime nowUtc, CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .CountAsync(b =>
                    b.BusinessId == businessId &&
                    b.StartTimeUtc > nowUtc &&
                    b.Status != BookingStatus.Cancelled,
                    ct);
        }

        public async Task<int> CountByStatusForBusinessAsync(Guid businessId, BookingStatus status, CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .CountAsync(b =>
                    b.BusinessId == businessId &&
                    b.Status == status,
                    ct);
        }

        public async Task<int> CountInRangeForBusinessAsync(
            Guid businessId,
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .CountAsync(b =>
                    b.BusinessId == businessId &&
                    b.StartTimeUtc >= startUtc &&
                    b.StartTimeUtc < endUtc,
                    ct);
        }

        public async Task<bool> UpdateStatusAsync(
            Guid businessId,
            Guid bookingId,
            BookingStatus newStatus,
            CancellationToken ct)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b =>
                    b.BusinessId == businessId &&
                    b.Id == bookingId,
                    ct);

            if (booking is null)
                return false;

            booking.Status = newStatus;
            booking.UpdatedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

    }
}
