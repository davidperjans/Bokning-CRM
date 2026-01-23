using Application.Admin.DTOs;
using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Admin.Commands.CreateStaff
{
    public sealed record CreateStaffCommand(
        Guid BusinessId,
        string Name,
        string Title,
        string? ImageUrl,
        string? Bio,
        bool IsActive,
        IReadOnlyCollection<Guid> QualifiedServiceIds
    ) : IRequest<OperationResult<StaffDto>>;
}
