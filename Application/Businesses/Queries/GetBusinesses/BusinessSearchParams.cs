using Domain.Enum;

namespace Application.Businesses.Queries.GetBusinesses
{
    public sealed record BusinessSearchParams(
        string? City,
        string? Category,
        string? Query,
        BusinessSort Sort,
        int Page,
        int PageSize
    );
}
