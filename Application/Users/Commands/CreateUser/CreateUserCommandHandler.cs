using Application.Common;
using Application.Interface;
using Application.Users.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    // Application/Users/Commands/CreateUser/CreateUserHandler.cs
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, OperationResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Validering (Affärslogik)
            if (await _userRepository.GetByEmailAsync(request.Email) != null)
                return OperationResult<UserDto>.Failure("E-postadressen används redan.");

            // 2. Mappa Command -> Entity (Manuell mapping)
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                DateOfBirth = request.DateOfBirth,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Spara
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // 4. Mappa Entity -> DTO (Manuell mapping för respons)
            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                user.DateOfBirth,
                user.Role.ToString(),
                user.CreatedAt
            );

            return OperationResult<UserDto>.Success(userDto);
        }
    }
}