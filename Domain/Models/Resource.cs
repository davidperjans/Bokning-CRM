
using Domain.Enum;

namespace Domain.Models
{
    public sealed class Resource
    {
        public Guid Id { get; set; } // PK

        public Guid BusinessId { get; set; } // Ägare (FK till Business.Id)

        public string Name { get; set; } = string.Empty; 

        public string? Description { get; set; } 

        public BookableResourceType Type { get; set; } // Room / Equipment / Person / Other

        public int? Capacity { get; set; } 

        public string? Location { get; set; } 

        public bool IsActive { get; set; } 

        public string? ImageUrl { get; set; } 

        public DateTime CreatedAtUtc { get; set; } 

        public DateTime UpdatedAtUtc { get; set; } 

        public bool IsDeleted { get; set; } 
    }
}
