using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class BusinessConfig : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.ToTable("businesses");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(b => b.Slug)
                .HasColumnName("slug")
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(b => b.Slug)
                .IsUnique();

            builder.Property(b => b.Description)
                .HasColumnName("description")
                .HasMaxLength(2000);

            builder.Property(b => b.Address)
                .HasColumnName("address")
                .HasMaxLength(500);

            builder.Property(b => b.City)
                .HasColumnName("city")
                .HasMaxLength(100);

            builder.HasIndex(b => b.City);

            builder.Property(b => b.Category)
                .HasColumnName("category")
                .HasMaxLength(100);

            builder.HasIndex(b => b.Category);

            builder.Property(b => b.Rating)
                .HasColumnName("rating")
                .HasDefaultValue(0.0);

            builder.Property(b => b.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500);

            builder.Property(b => b.OwnerId)
                .HasColumnName("owner_id")
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Property(b => b.UpdatedAt)
                .IsRequired();

            // Relationships
            // TODO: Add relationships when we have implemented related entities


            builder.HasOne(b => b.Owner)
                .WithMany(u => u.Businesses)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Services)
                .WithOne(s => s.Business)
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(b => b.Staff)
            //    .WithOne(s => s.Business)
            //    .HasForeignKey(s => s.BusinessId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(b => b.Reviews)
            //    .WithOne(r => r.Business)
            //    .HasForeignKey(r => r.BusinessId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
