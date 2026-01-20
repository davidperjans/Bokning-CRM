using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.Queries.GetDetailsBySlug
{
    public class GetDetailsBySlugQueryHandler : IRequestHandler<GetDetailsBySlugQuery, OperationResult<BusinessDetailsDto>>
    {
        private readonly IBusinessRepository _repository;
        public GetDetailsBySlugQueryHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<BusinessDetailsDto>> Handle(GetDetailsBySlugQuery request, CancellationToken cancellationToken)
        {
            var dto = await _repository.GetDetailsBySlugAsync(request.Slug, cancellationToken);

            if (dto == null) 
                return OperationResult<BusinessDetailsDto>.Failure("business not found.");

            return OperationResult<BusinessDetailsDto>.Success(dto);
        }
    }
}
