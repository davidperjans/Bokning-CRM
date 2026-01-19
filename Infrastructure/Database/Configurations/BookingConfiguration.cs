using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            // PK
            builder.HasKey(x => x.Id);

            // FK: BookingTypeId -> BookingTypes
            builder.HasOne<BookingType>()
                .WithMany()
                .HasForeignKey(x => x.BookingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // FK: ResourceId -> Resources (valfri)
            builder.HasOne<Resource>()
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index för overlap-check och listningar per resurs
            builder.HasIndex(x => new { x.BusinessId, x.ResourceId, x.StartTimeUtc, x.EndTimeUtc });

            // Index för "max bookings per user" och listning per användare/status
            builder.HasIndex(x => new { x.BusinessId, x.UserId, x.Status });
        }
    }
}
