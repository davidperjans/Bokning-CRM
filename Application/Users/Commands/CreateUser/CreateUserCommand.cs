using Application.Common;
using Application.Users.Dtos;
using Domain.Enum;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Phone,
        DateTime? DateOfBirth,
        UserRole Role = UserRole.User) : IRequest<OperationResult<UserDto>>; // Detta är standardvärdet för rollen

}