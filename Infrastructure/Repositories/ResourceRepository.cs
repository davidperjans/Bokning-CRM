using Application.Interface;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppDbContext _context;

        public ResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Resource?> GetByIdAsync(Guid businessId, Guid resourceId, CancellationToken ct)
        {
            // Hämtar resurs inom rätt business (soft delete-filter gäller om konfigurerat)
            return await _context.Resources
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.Id == resourceId &&
                    r.BusinessId == businessId,
                    ct);
        }

        public async Task<bool> IsAllowedForBookingTypeAsync(
            Guid bookingTypeId,
            Guid resourceId,
            CancellationToken ct)
        {
            // Kollar junction-tabellen BookingTypeResources
            return await _context.BookingTypeResources
                .AsNoTracking()
                .AnyAsync(x =>
                    x.BookingTypeId == bookingTypeId &&
                    x.ResourceId == resourceId,
                    ct);
        }

        public async Task<List<Resource>> GetResourcesForBookingTypeAsync(
            Guid businessId,
            Guid bookingTypeId,
            CancellationToken ct)
        {
            var resourceIds = await _context.BookingTypeResources
                .Where(x => x.BookingTypeId == bookingTypeId)
                .Select(x => x.ResourceId)
                .ToListAsync(ct);

            return await _context.Resources
                .AsNoTracking()
                .Where(r =>
                    r.BusinessId == businessId &&
                    r.IsActive &&
                    !r.IsDeleted &&
                    resourceIds.Contains(r.Id))
                .ToListAsync(ct);
        }

    }
}
