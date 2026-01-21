namespace Domain.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Herrklippning"
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public Business Business { get; set; } = null!;
    }
}
