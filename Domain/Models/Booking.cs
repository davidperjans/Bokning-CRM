
using Domain.Enum;

namespace Domain.Models
{
    public sealed class Booking
    {
        public Guid Id { get; set; } // PK

        // Ägarkontext
        public Guid BusinessId { get; set; } // FK till Business

        // Vem som bokar
        public Guid UserId { get; set; }

        // Vad som bokas
        public Guid BookingTypeId { get; set; }
        public Guid? ResourceId { get; set; } // Nullable om typ inte kräver resurs

        // Tid 
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }

        // Status
        public BookingStatus Status { get; set; }

        // Extra info
        public string? Notes { get; set; }

        // Avbokning
        public DateTime? CancelledAtUtc { get; set; }
        public Guid? CancelledByUserId { get; set; }
        public string? CancellationReason { get; set; }

        // Metadata
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}
