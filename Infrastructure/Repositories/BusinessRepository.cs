using Application.Businesses.DTOs;
using Application.Businesses.Queries.GetBusinesses;
using Application.Common;
using Application.Interface;
using Application.Reviews.DTOs;
using Application.Services.DTOs;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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

        public async Task<BusinessWithServicesDto?> GetWithServicesBySlugAsync(string slug, CancellationToken ct)
        {
            return await _context.Businesses
            .AsNoTracking()
            .Where(b => b.Slug == slug)
            .Select(b => new BusinessWithServicesDto
            {
                Business = new BusinessSummaryDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Slug = b.Slug
                },
                Services = b.Services
                    .OrderBy(s => s.Name)
                    .Select(s => new ServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Price = s.Price,
                        DurationMinutes = s.DurationMinutes,
                        IsActive = s.IsActive
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
        }

        public async Task<BusinessWithReviewsDto?> GetWithReviewsBySlugAsync(string slug, CancellationToken ct)
        {
            return await _context.Businesses
            .AsNoTracking()
            .Where(b => b.Slug == slug)
            .Select(b => new BusinessWithReviewsDto
            {
                Business = new BusinessSummaryDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Slug = b.Slug
                },
                Reviews = b.Reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        Rating = r.Rating,
                        FullName = (r.User.FirstName + " " + r.User.LastName).Trim(),
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
        }

        public async Task<PagedResult<BusinessListItemDto>> SearchAsync(BusinessSearchParams p, CancellationToken ct)
        {
            IQueryable<Business> q = _context.Businesses.AsNoTracking();

            // Filters (case-insensitive)
            if (!string.IsNullOrWhiteSpace(p.City))
                q = q.Where(b => EF.Functions.ILike(b.City, p.City));

            if (!string.IsNullOrWhiteSpace(p.Category))
                q = q.Where(b => EF.Functions.ILike(b.Category, p.Category));

            // Search query: matcha flera fält med wildcard
            if (!string.IsNullOrWhiteSpace(p.Query))
            {
                var term = p.Query.Trim();

                // %term% = "contains"
                var pattern = $"%{term}%";

                q = q.Where(b =>
                    EF.Functions.ILike(b.Name, pattern) ||
                    (b.Description != null && EF.Functions.ILike(b.Description, pattern)) ||
                    (b.Address != null && EF.Functions.ILike(b.Address, pattern)));
            }

            // Sorting (stabil tie-breaker)
            q = p.Sort switch
            {
                BusinessSort.RatingDesc => q.OrderByDescending(b => b.Rating).ThenBy(b => b.Id),
                BusinessSort.Newest => q.OrderByDescending(b => b.CreatedAt).ThenBy(b => b.Id),
                _ => q.OrderBy(b => b.Name).ThenBy(b => b.Id),
            };

            var totalCount = await q.CountAsync(ct);

            var items = await q
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .Select(b => new BusinessListItemDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Slug = b.Slug,
                    City = b.City,
                    Category = b.Category,
                    Rating = b.Rating,
                    ImageUrl = b.ImageUrl
                })
                .ToListAsync(ct);

            return new PagedResult<BusinessListItemDto>(items, p.Page, p.PageSize, totalCount);
        }
        
        public async Task<List<Business>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Businesses
                .Include(b => b.Owner) // VIKTIGT: Hämtar användardatan kopplad till företaget
                .AsNoTracking()        // Snabbare sökning eftersom vi bara ska läsa data
                .OrderByDescending(b => b.CreatedAt) // Visar de nyaste företagen först
                .ToListAsync(cancellationToken);
        }
        
        
    }
}
