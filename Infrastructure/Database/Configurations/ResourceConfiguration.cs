using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Database.Configurations
{
    public sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("Resources");

            builder.HasKey(x => x.Id);

            // Soft delete-filter
            builder.HasQueryFilter(x => !x.IsDeleted);

            // Index för vanliga queries
            builder.HasIndex(x => x.BusinessId);
            builder.HasIndex(x => new { x.BusinessId, x.IsActive });
            builder.HasIndex(x => new { x.BusinessId, x.Type });

            // (Valfritt) begränsa namn
            builder.Property(x => x.Name)
                   .HasMaxLength(200);
        }
    }
}
