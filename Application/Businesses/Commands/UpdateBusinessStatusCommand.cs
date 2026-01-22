using Application.Common;
using Domain.Enum;
using MediatR;

namespace Application.Businesses.Commands
{
    public record UpdateBusinessStatusCommand(Guid Id, BusinessStatus NewStatus) : IRequest<OperationResult<bool>>;
}