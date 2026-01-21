using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.Queries.GetReviewsForBusiness
{
    public class GetReviewsForBusinessQueryHandler : IRequestHandler<GetReviewsForBusinessQuery, OperationResult<BusinessWithReviewsDto>>
    {
        private readonly IBusinessRepository _repository;
        public GetReviewsForBusinessQueryHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }
        public async Task<OperationResult<BusinessWithReviewsDto>> Handle(GetReviewsForBusinessQuery request, CancellationToken cancellationToken)
        {
            var dto = await _repository.GetWithReviewsBySlugAsync(request.Slug, cancellationToken);

            if (dto == null)
                return OperationResult<BusinessWithReviewsDto>.Failure("business not found.");

            return OperationResult<BusinessWithReviewsDto>.Success(dto);
        }
    }
}
