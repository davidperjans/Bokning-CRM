namespace Domain.Models
{
    public class Staff
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; } // e.g., "Frisör", "Terapeut"
        public string ImageUrl { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }
        
        public Business Business { get; set; } = null!;

        // Which services can this staff perform?
        public ICollection<Service> QualifiedServices { get; set; }
        public List<WorkingHour> WorkingHours { get; set; }
    }
}
