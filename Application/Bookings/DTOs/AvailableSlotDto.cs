namespace Application.Bookings.DTOs
{
    public class AvailableSlotDto
    {
        public Guid ResourceId { get; set; }

        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
    }
}
