using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Businesses.Queries.GetServicesForBusiness
{
    public class GetServicesForBusinessQueryHandler : IRequestHandler<GetServicesForBusinessQuery, OperationResult<BusinessWithServicesDto>>
    {
        private readonly IBusinessRepository _repository;
        public GetServicesForBusinessQueryHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }
        public async Task<OperationResult<BusinessWithServicesDto>> Handle(GetServicesForBusinessQuery request, CancellationToken cancellationToken)
        {
            var dto = await _repository.GetWithServicesBySlugAsync(request.Slug, cancellationToken);

            if (dto == null)
                return OperationResult<BusinessWithServicesDto>.Failure("business not found.");

            return OperationResult<BusinessWithServicesDto>.Success(dto);
        }
    }
}
