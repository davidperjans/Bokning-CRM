using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.DTOs;


namespace Application.Businesses.DTOs
{
    public class BusinessWithServicesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; 
        public List<ServiceDto> Services { get; set; } = new();
    }
}
