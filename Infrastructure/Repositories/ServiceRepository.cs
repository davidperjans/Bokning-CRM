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
        
        public async Task<Service?> GetServiceByIdAsync(Guid serviceId, CancellationToken ct)
        {
            // Vi hämtar tjänsten för att kunna uppdatera den
            return await _db.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId, ct);
        }

        public async Task UpdateServiceAsync(Service service, CancellationToken ct)
        {
            _db.Services.Update(service);
            
            await _db.SaveChangesAsync(ct);
        }
    }
}
