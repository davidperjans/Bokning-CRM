using Application.Common;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<OperationResult<LoginResponseDto>>;
}