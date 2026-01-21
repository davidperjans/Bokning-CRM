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
        public BusinessSummaryDto Business { get; set; } = new();
        public List<ServiceDto> Services { get; set; } = new();
    }
}
