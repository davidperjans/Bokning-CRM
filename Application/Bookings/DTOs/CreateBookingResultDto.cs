using Domain.Enum;

namespace Application.Bookings.DTOs
{
    public class CreateBookingResultDto
    {
        public Guid BookingId { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
    }
}
