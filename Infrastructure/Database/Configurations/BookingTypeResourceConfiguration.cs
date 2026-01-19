using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public sealed class BookingTypeResourceConfiguration : IEntityTypeConfiguration<BookingTypeResource>
    {
        public void Configure(EntityTypeBuilder<BookingTypeResource> builder)
        {
            // Composite primary key (PK)
            builder.HasKey(x => new { x.BookingTypeId, x.ResourceId });

            // Table name (valfritt, men nice att vara tydlig)
            builder.ToTable("BookingTypeResources");

            // FK till BookingTypes
            builder.HasOne<BookingType>()
                .WithMany()
                .HasForeignKey(x => x.BookingTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // FK till Resources
            builder.HasOne<Resource>()
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index (snabbare queries)
            builder.HasIndex(x => x.BookingTypeId);
            builder.HasIndex(x => x.ResourceId);
        }
    }
}
