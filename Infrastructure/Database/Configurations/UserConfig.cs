using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);

            builder.Property(u => u.Role)
                .HasColumnName("role")
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired(false);

            // Navigation - User can own multiple businesses
            builder.HasMany(u => u.Businesses)
                .WithOne(b => b.Owner)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
