using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Description)
                .HasMaxLength(2000);

            builder.Property(s => s.DurationMinutes)
                .IsRequired();

            builder.Property(s => s.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(s => s.BusinessId);
            builder.HasIndex(s => new { s.BusinessId, s.IsActive });

            // Relation: Business (1) -> Services (many)
            builder.HasOne<Business>() // eller .HasOne(s => s.Business) om du har nav-prop
                .WithMany(b => b.Services) // kräver att Business har ICollection<Service> Services
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
