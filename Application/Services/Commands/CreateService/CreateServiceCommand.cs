using Application.Common;
using MediatR;

namespace Application.Services.Commands.CreateService
{
    public record CreateServiceCommand(
        Guid BusinessId,
        string Name,
        string Description,
        int DurationMinutes,
        decimal Price,
        bool IsActive = true
        ) : IRequest<OperationResult<Guid>>;
}