using Application.Businesses.DTOs;
using Application.Common;

namespace Application.Interface
{
    public interface IBusinessRepository
    {
        Task<BusinessDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct);
    }
}
