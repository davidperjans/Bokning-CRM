using Domain.Models;

namespace Application.Interface
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetByIdsAsync(IReadOnlyCollection<Guid> serviceIds, CancellationToken ct);
    }
}
