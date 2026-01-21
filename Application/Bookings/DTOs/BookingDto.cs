using Domain.Enum;

namespace Application.Bookings.Dtos
{
    public class BookingDto
    {
        public Guid Id { get; set; }

        public Guid BusinessId { get; set; }
        public Guid UserId { get; set; }

        public Guid BookingTypeId { get; set; }
        public Guid? ResourceId { get; set; }

        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }

        public BookingStatus Status { get; set; }

        public string? Notes { get; set; }
    }
}
