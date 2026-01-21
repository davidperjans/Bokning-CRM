using Application.Common;
using MediatR;

namespace Application.Users.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<OperationResult<bool>>;
}