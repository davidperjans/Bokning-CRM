using Domain.Models;

namespace Application.Interface
{
    public interface IResourceRepository
    {
        Task<Resource?> GetByIdAsync(Guid businessId, Guid resourceId, CancellationToken ct);

        // Kollar junction-tabellen: får denna booking type bokas på denna resurs?
        Task<bool> IsAllowedForBookingTypeAsync(Guid bookingTypeId, Guid resourceId, CancellationToken ct);
    }
}
