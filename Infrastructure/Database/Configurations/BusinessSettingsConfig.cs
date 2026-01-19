using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{
    public class BusinessSettingsConfig : IEntityTypeConfiguration<BusinessSettings>
    {
        public void Configure(EntityTypeBuilder<BusinessSettings> builder)
        {
            builder.ToTable("business_settings");

            // BusinessId is both the PK and FK (1:1 relationship)
            builder.HasKey(bs => bs.BusinessId);

            builder.Property(bs => bs.BusinessId)
                .HasColumnName("business_id");

            builder.Property(bs => bs.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .HasDefaultValue("SEK")
                .IsRequired();

            builder.Property(bs => bs.TimeZone)
                .HasColumnName("time_zone")
                .HasMaxLength(50)
                .HasDefaultValue("Europe/Stockholm")
                .IsRequired();

            // 1:1 relationship with Business
            builder.HasOne<Business>()
                .WithOne()
                .HasForeignKey<BusinessSettings>(bs => bs.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
