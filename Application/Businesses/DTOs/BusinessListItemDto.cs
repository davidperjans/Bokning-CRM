using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.DTOs
{
    public sealed class BusinessListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public string City { get; set; } = "";
        public string Category { get; set; } = "";
        public double Rating { get; set; }
        public string? ImageUrl { get; set; }
    }
}
