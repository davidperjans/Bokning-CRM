using Application.Common;
using MediatR;

namespace Application.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Token, string NewPassword): IRequest<OperationResult<bool>>;
}