using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _context;

        public BusinessRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessDetailsDto?> GetDetailsBySlugAsync(string slug, CancellationToken ct)
        {
            return await _context.Businesses
                .AsNoTracking()
                .Where(b => b.Slug == slug)
                .Select(b => new BusinessDetailsDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Slug = b.Slug,
                    Description = b.Description,
                    Address = b.Address,
                    City = b.City,
                    Category = b.Category,
                    Rating = b.Rating,
                    ImageUrl = b.ImageUrl,
                    CreatedAt = b.CreatedAt
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
