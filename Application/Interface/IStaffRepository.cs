using Domain.Models;

namespace Application.Interface
{
    public interface IStaffRepository
    {
        Task AddAsync(Staff staff, CancellationToken ct);
    }
}
