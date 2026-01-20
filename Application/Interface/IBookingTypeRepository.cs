using Domain.Models;

namespace Application.Interface
{
    public interface IBookingTypeRepository
    {
        // Hämtar en BookingType inom en Business (viktigt för multi-tenant)
        Task<BookingType?> GetByIdAsync(Guid businessId, Guid bookingTypeId, CancellationToken ct);
    }
}
