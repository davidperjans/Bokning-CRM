
namespace Application.Bookings.DTOs
{
    public class CancelBookingResultDto
    {
        public Guid BookingId { get; set; }
        public bool WasCancelled { get; set; }
    }
}
