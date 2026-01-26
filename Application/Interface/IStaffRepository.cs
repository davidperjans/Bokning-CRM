using Domain.Models;

namespace Application.Interface
{
    public interface IStaffRepository
    {
        Task AddAsync(Staff staff, CancellationToken ct);
        
        Task<Staff?> GetByIdAsync(Guid staffId, CancellationToken ct);
        
        Task UpdateScheduleAsync(Guid staffId, List<WorkingHour> days, CancellationToken ct);
        Task<List<Staff>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct);
    }
}
