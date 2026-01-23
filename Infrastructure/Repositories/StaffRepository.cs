using Application.Interface;
using Domain.Models;
using Infrastructure.Database;
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
    }
}
