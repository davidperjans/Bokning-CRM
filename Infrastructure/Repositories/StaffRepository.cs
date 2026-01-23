using Application.Interface;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public sealed class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _db;

        public StaffRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Staff staff, CancellationToken ct)
        {
            await _db.Staffs.AddAsync(staff, ct);
            await _db.SaveChangesAsync(ct);
        }
        
        public async Task<Staff?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Staffs
                .Include(s => s.Business) 
                .Include(s => s.WorkingHours)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task UpdateScheduleAsync(Guid staffId, List<WorkingHour> days, CancellationToken ct)
        {
            var staff = await _db.Staffs
                .Include(s => s.WorkingHours)
                .FirstOrDefaultAsync(s => s.Id == staffId, ct);

            if (staff != null)
            {
                _db.WorkingHours.RemoveRange(staff.WorkingHours);
            
                staff.WorkingHours = days;
            
                await _db.SaveChangesAsync(ct);
            }
        }

        public async Task<List<Staff>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct)
        {
            return await _db.Staffs
                .Where(s => s.BusinessId == businessId)
                .ToListAsync(ct);
        }
    }
}
