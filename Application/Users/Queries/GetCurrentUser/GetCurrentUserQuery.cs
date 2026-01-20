using Application.Common;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery(): IRequest<OperationResult<UserDto>>;
}