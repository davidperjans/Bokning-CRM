using Application.Common;
using Application.SuperAdmin.Dtos;
using MediatR;

namespace Application.Businesses.Queries.GetAllBusinesses
{
    public record GetAllBusinessesQuery() : IRequest<OperationResult<List<BusinessAdminListItemDto>>>;
    
}