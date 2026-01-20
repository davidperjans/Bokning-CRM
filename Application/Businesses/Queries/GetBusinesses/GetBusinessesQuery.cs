using Application.Businesses.DTOs;
using Application.Common;
using Domain.Enum;
using MediatR;

namespace Application.Businesses.Queries.GetBusinesses
{
    public sealed record GetBusinessesQuery(
        string? City,
        string? Category,
        string? Query,
        BusinessSort Sort = BusinessSort.NameAsc,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<OperationResult<PagedResult<BusinessListItemDto>>>;
}
