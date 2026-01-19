namespace Domain.Models
{
    public class BusinessSettings
    {
        public Guid BusinessId { get; set; }
        public string Currency { get; set; } = "SEK";
        public string TimeZone { get; set; } = "Europe/Stockholm";
        // Booking rules, cancellation policies etc.
    }
}
