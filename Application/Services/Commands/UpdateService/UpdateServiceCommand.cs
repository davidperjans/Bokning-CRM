using Application.Common;
using Application.Services.DTOs;
using MediatR;

namespace Application.Services.Commands.UpdateService
{
    public record UpdateServiceCommand(
        Guid Id, // Id från URL:en
        string Name,
        string Description,
        int DurationMinutes,
        decimal Price,
        bool IsActive
    ) : IRequest<OperationResult<UpdateServiceDto>>;

    
}