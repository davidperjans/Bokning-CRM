using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public sealed class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()"); // Postgres

            builder.HasIndex(r => r.BusinessId);
            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => new { r.BusinessId, r.CreatedAt });

            // Business (1) -> Reviews (many)
            builder.HasOne<Business>() // eller .HasOne(r => r.Business) om du har nav prop
                .WithMany(b => b.Reviews) // kräver ICollection<Review> Reviews i Business
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // User (1) -> Reviews (many)
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: constraint 1-5 (Postgres CHECK)
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Reviews_Rating_Range", "\"Rating\" >= 1 AND \"Rating\" <= 5");
            });
        }
    }
}
