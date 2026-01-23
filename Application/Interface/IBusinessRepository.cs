using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetBusinesses;
using Application.Common;
using Domain.Models;

namespace Application.Interface
{
    public interface IBusinessRepository
    {
        Task<BusinessDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct);
        Task<PagedResult<BusinessListItemDto>> SearchAsync(BusinessSearchParams p, CancellationToken ct);
        Task<BusinessWithServicesDto?> GetWithServicesBySlugAsync(string slug, CancellationToken ct);
        Task<BusinessWithReviewsDto?> GetWithReviewsBySlugAsync(string slug, CancellationToken ct);
        Task<List<Business>> GetAllAsync(CancellationToken cancellationToken = default);
        
        Task<Business?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Business business, CancellationToken cancellationToken = default);
        
        Task AddServiceAsync(Service service, CancellationToken cancellationToken = default);
    }
}
