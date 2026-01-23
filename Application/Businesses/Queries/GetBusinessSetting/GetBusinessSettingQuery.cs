using Application.Businesses.DTOs;
using Application.Common;
using MediatR;

namespace Application.Businesses.Queries.GetBusinessSetting
{
    public record GetBusinessSettingsQuery() : IRequest<OperationResult<BusinessSettingsDto>>;
    
}