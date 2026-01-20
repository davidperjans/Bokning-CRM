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
    }
}
