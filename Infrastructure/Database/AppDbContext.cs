using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BusinessSettings> BusinessSettings { get; set; }
        public DbSet<BookingType> BookingTypes { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingTypeResource> BookingTypeResources { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<WorkingHour> WorkingHours { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State is not (EntityState.Added or EntityState.Modified))
                    continue;

                // Hoppa över Owned/Join entities om du vill (valfritt)
                // if (entry.Metadata.IsOwned()) continue;

                // UPDATED
                var updatedAtProp = entry.Metadata.FindProperty("UpdatedAt");
                if (updatedAtProp != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = utcNow;
                }

                // CREATED (bara vid Added)
                if (entry.State == EntityState.Added)
                {
                    var createdAtProp = entry.Metadata.FindProperty("CreatedAt");
                    if (createdAtProp != null)
                    {
                        entry.Property("CreatedAt").CurrentValue = utcNow;
                    }
                }
                else
                {
                    // Skydda CreatedAt från att ändras vid update (om det finns)
                    var createdAtProp = entry.Metadata.FindProperty("CreatedAt");
                    if (createdAtProp != null)
                    {
                        entry.Property("CreatedAt").IsModified = false;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
