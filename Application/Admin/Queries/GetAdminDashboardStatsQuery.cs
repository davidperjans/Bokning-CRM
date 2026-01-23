using Application.Admin.DTOs;
using Application.Common;
using MediatR;

namespace Application.Admin.Queries
{
    public sealed record GetAdminDashboardStatsQuery
        : IRequest<OperationResult<AdminDashboardStatsDto>>;
}
