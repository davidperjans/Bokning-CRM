using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class StaffConfig : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("staff");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.BusinessId)
                .HasColumnName("business_id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(x => x.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500);

            builder.Property(x => x.Bio)
                .HasColumnName("bio")
                .HasMaxLength(2000);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            // Index för snabb lookup per business + aktiva
            builder.HasIndex(x => new { x.BusinessId, x.IsActive })
                .HasDatabaseName("ix_staff_business_active");

            builder.HasOne<Business>()
                .WithMany()
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many: Staff <-> Service via join-table
            builder
                .HasMany(x => x.QualifiedServices)
                .WithMany() // byt till .WithMany(s => s.QualifiedStaff) om du har nav på Service
                .UsingEntity<Dictionary<string, object>>(
                    "staff_service",
                    r => r.HasOne<Service>()
                          .WithMany()
                          .HasForeignKey("service_id")
                          .HasPrincipalKey(nameof(Service.Id))
                          .OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<Staff>()
                          .WithMany()
                          .HasForeignKey("staff_id")
                          .HasPrincipalKey(nameof(Staff.Id))
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.ToTable("staff_service");

                        j.HasKey("staff_id", "service_id");

                        j.Property<Guid>("staff_id").HasColumnName("staff_id");
                        j.Property<Guid>("service_id").HasColumnName("service_id");

                        j.HasIndex("service_id").HasDatabaseName("ix_staff_service_service_id");
                    });
            
           
        }
    }
}
