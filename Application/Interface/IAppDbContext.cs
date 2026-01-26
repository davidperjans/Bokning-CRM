using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Business> Businesses { get; }
        public DbSet<Service> Services { get; }
        public DbSet<Review> Reviews { get; }
        public DbSet<BusinessSettings> BusinessSettings { get; }
        public DbSet<BookingType> BookingTypes { get; }
        public DbSet<Resource> Resources { get; }
        public DbSet<Booking> Bookings { get; }
        public DbSet<BookingTypeResource> BookingTypeResources { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }
        public DbSet<Staff> Staffs { get; }
        public DbSet<Schedule> Schedules { get; }
        public DbSet<WorkingHour> WorkingHours { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
