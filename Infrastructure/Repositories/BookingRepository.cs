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
    }
}
