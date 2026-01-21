using Application.Common;
using Application.Interface;
using Application.SuperAdmin.Dtos; // Se till att denna import stämmer
using Domain;
using MediatR;

namespace Application.Businesses.Queries.GetAllBusinesses
{
    // 1. Returnera OperationResult<List<BusinessAdminListItemDto>> för att matcha controllern
    public class GetAllBusinessesHandler : IRequestHandler<GetAllBusinessesQuery, OperationResult<List<BusinessAdminListItemDto>>>
    {
        private readonly IBusinessRepository _businessRepository;
        
        public GetAllBusinessesHandler(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }

        public async Task<OperationResult<List<BusinessAdminListItemDto>>> Handle(GetAllBusinessesQuery request,
            CancellationToken cancellationToken)
        {
            // 2. Se till att GetAllAsync finns i IBusinessRepository
            var businesses = await _businessRepository.GetAllAsync(cancellationToken);
            
            // 3. Mappa till BusinessAdminListItemDto (inte BusinessDto)
            var businessDtos = businesses.Select(b => new BusinessAdminListItemDto(
                b.Id,
                b.Name,
                b.Status.ToString(),
                b.CreatedAt,
                b.Owner?.Email ?? "No Owner"
            )).ToList();
            
            // 4. Returnera rätt typ
            return OperationResult<List<BusinessAdminListItemDto>>.Success(businessDtos);
        }
    }
}