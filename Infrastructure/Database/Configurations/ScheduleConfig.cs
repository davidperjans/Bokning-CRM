using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("schedules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.StaffId)
                .HasColumnName("staff_id")
                .IsRequired();

            // DayOfWeek (0-6) -> smallint i Postgres
            builder.Property(x => x.DayOfWeek)
                .HasColumnName("day_of_week")
                .HasConversion<short>()
                .HasColumnType("smallint")
                .IsRequired();

            // TimeSpan -> time without time zone
            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .HasColumnType("time without time zone")
                .IsRequired();

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .HasColumnType("time without time zone")
                .IsRequired();

            builder.Property(x => x.IsDayOff)
                .HasColumnName("is_day_off")
                .IsRequired()
                .HasDefaultValue(false);

            // En rad per staff + veckodag (vanligast)
            builder.HasIndex(x => new { x.StaffId, x.DayOfWeek })
                .IsUnique()
                .HasDatabaseName("ux_schedules_staff_dayofweek");

            // FK -> Staff
            builder.HasOne<Staff>()
                .WithMany()
                .HasForeignKey(x => x.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enkla constraints (Postgres)
            builder.ToTable(t =>
            {
                // EndTime efter StartTime när inte day off
                t.HasCheckConstraint(
                    "ck_schedules_time_range",
                    "\"is_day_off\" = true OR \"end_time\" > \"start_time\""
                );

                // DayOfWeek inom 0..6
                t.HasCheckConstraint(
                    "ck_schedules_dayofweek_range",
                    "\"day_of_week\" >= 0 AND \"day_of_week\" <= 6"
                );
            });
        }
    }
}
