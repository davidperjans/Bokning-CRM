using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class WorkingHourConfig : IEntityTypeConfiguration<WorkingHour>
    {
        public void Configure(EntityTypeBuilder<WorkingHour> builder)
        {
            builder.HasKey(w => w.Id);

            // Konfigurera relationen till Staff
            builder.HasOne(w => w.Staff)
                .WithMany(s => s.WorkingHours)
                .HasForeignKey(w => w.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mappa DayOfWeek som int i databasen
            builder.Property(w => w.DayOfWeek)
                .IsRequired();

            // Om du använder PostgreSQL mappar TimeSpan automatiskt till 'time'
            builder.Property(w => w.StartTime)
                .HasColumnType("time without time zone");

            builder.Property(w => w.EndTime)
                .HasColumnType("time without time zone");
        }
    }
}