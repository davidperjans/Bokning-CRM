using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using Application.Services.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Businesses.Queries.GetServicesForBusiness
{
    public class GetServicesForBusinessQueryHandler : IRequestHandler<GetServicesForBusinessQuery, OperationResult<BusinessWithServicesDto>>
    {
        private readonly IAppDbContext _db;
        public GetServicesForBusinessQueryHandler(IAppDbContext db)
        {
            _db = db;
        }
        public async Task<OperationResult<BusinessWithServicesDto>> Handle(GetServicesForBusinessQuery request, CancellationToken cancellationToken)
        {
            var dto = await _db.Businesses
                .AsNoTracking()
                .Where(b => b.Slug == request.Slug)
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
                    .Select(s => new ServiceDto(
                        s.Id,
                        s.Name,
                        s.Description,
                        s.DurationMinutes,
                        s.Price,
                        s.IsActive,
                        b.Id
                    ))
                    .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return OperationResult<BusinessWithServicesDto>.Failure("business not found.");

            return OperationResult<BusinessWithServicesDto>.Success(dto);
        }
    }
}
