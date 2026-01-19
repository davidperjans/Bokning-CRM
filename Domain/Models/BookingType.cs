
namespace Domain.Models
{
    public sealed class BookingType
    {
        public Guid Id { get; set; }

        // Ägarkontext – koppling till Business (tenant)
        public Guid BusinessId { get; set; }

        // Visningsdata
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Bokningsregler
        public int DurationMinutes { get; set; }
        public int MaxAdvanceBookingDays { get; set; }
        public int? MaxBookingsPerUser { get; set; }
        public bool RequiresApproval { get; set; }

        // Status / UI
        public bool IsActive { get; set; }
        public string? Color { get; set; }

        // Metadata
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
