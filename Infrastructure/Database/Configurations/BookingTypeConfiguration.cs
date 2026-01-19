using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public sealed class BookingTypeConfiguration : IEntityTypeConfiguration<BookingType>
    {
        public void Configure(EntityTypeBuilder<BookingType> builder)
        {
            builder.ToTable("BookingTypes");

            // PK
            builder.HasKey(x => x.Id);

            // Soft delete-filter: EF kommer automatiskt ignorera rader där IsDeleted = true
            builder.HasQueryFilter(x => !x.IsDeleted);

            // Index för vanliga queries: "alla typer för business" + "bara aktiva"
            builder.HasIndex(x => x.BusinessId);
            builder.HasIndex(x => new { x.BusinessId, x.IsActive });

            // (Valfritt men ofta bra) Begränsa längd på Name så du slipper gigantiska strängar i DB
            builder.Property(x => x.Name)
                   .HasMaxLength(200);
        }
    }
}
