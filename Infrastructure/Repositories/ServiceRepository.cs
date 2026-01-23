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
    public sealed class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _db;

        public ServiceRepository(AppDbContext db) => _db = db;

        public Task<List<Service>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct)
        {
            return _db.Services
                .Where(s => ids.Contains(s.Id))
                .ToListAsync(ct);
        }
    }
}
