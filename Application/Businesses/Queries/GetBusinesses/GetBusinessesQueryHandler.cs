using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Businesses.Queries.GetBusinesses
{
    public class GetBusinessesQueryHandler : IRequestHandler<GetBusinessesQuery, OperationResult<PagedResult<BusinessListItemDto>>>
    {
        private readonly IBusinessRepository _repository;
        public GetBusinessesQueryHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }
        public async Task<OperationResult<PagedResult<BusinessListItemDto>>> Handle(GetBusinessesQuery request, CancellationToken cancellationToken)
        {
            var p = new BusinessSearchParams(
                request.City,
                request.Category,
                request.Query,
                request.Sort,
                request.Page,
                request.PageSize
            );

            var paged = await _repository.SearchAsync(p, cancellationToken);
            return OperationResult<PagedResult<BusinessListItemDto>>.Success(paged);
        }
    }
}
