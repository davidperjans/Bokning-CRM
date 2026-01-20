using Application.Common;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Commands.LoginUser
{
    public record LoginCommand (string Email, string Password): IRequest<OperationResult<LoginResponseDto>>;
    
    
}