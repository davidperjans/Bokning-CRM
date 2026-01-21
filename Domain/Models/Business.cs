namespace Domain.Models
{
    public class Business
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; // For SEO-friendly URLs
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Could be an Enum or separate Entity
        public double Rating { get; set; } // Aggregated from reviews
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // TODO: Add navigation properties for related entities when they are implemented

        public ICollection<Service> Services { get; set; } = null!;
        //public ICollection<Staff> Staff { get; set; } = null!;
        //public ICollection<Review> Reviews { get; set; } = null!;
    }
}
