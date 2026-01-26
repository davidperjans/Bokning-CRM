using Application.Businesses.DTOs;
using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Businesses.Queries.GetBusinessSetting
{
    public class GetBusinessSettingsHandler : IRequestHandler<GetBusinessSettingsQuery, OperationResult<BusinessSettingsDto>>
    {
        private readonly IBusinessRepository _repository;
        private readonly IUserContext _userContext;

        public GetBusinessSettingsHandler(IBusinessRepository repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task<OperationResult<BusinessSettingsDto>> Handle(GetBusinessSettingsQuery request, CancellationToken ct)
        {
            var businessId = _userContext.BusinessId;
            if (businessId == null) return OperationResult<BusinessSettingsDto>.Failure("Användaren är inte kopplad till ett företag.");

            var business = await _repository.GetByIdAsync(businessId.Value, ct);
            if (business == null) return OperationResult<BusinessSettingsDto>.Failure("Företag hittades inte.");

            var dto = new BusinessSettingsDto(
                business.Id,
                business.Name,
                business.Description ?? "",
                business.ImageUrl,
                business.Category
            );

            return OperationResult<BusinessSettingsDto>.Success(dto);
        }
    }
}