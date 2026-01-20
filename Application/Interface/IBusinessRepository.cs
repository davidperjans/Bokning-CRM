using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetBusinesses;
using Application.Common;

namespace Application.Interface
{
    public interface IBusinessRepository
    {
        Task<BusinessDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct);
        Task<PagedResult<BusinessListItemDto>> SearchAsync(BusinessSearchParams p, CancellationToken ct);
    }
}
