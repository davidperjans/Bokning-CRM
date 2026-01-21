using Application.Common;
using Application.Interface;
using Application.Users.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Users.Commands.LoginUser
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<LoginResponseDto>> Handle(LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return OperationResult<LoginResponseDto>.Failure("Invalid email or password.");
            }

            // 1. Skapa Access Token (JWT)
            var token = _tokenService.GenerateJwtToken(user);

            // 2. Skapa Refresh Token sträng och sätt utgångstid
            var refreshTokenString = _tokenService.GenerateRefreshToken(user.Id);
            var expiryDate = DateTime.UtcNow.AddDays(7);

            // 3. Skapa och spara Refresh Token i databasen
            var refreshToken = new Domain.Models.RefreshToken
            {
                Token = refreshTokenString,
                UserId = user.Id,
                ExpiresAt = expiryDate,
                CreatedAt = DateTime.UtcNow
            };

            // Glöm inte att anropa ditt repository här för att faktiskt spara i DB!
            await _userRepository.SaveRefreshTokenAsync(refreshToken);

            // 4. Mappa användaren till DTO
            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                user.DateOfBirth,
                user.Role.ToString(),
                user.CreatedAt);


            var response = new LoginResponseDto(
                token,
                refreshTokenString,
                expiryDate,
                userDto);

            return OperationResult<LoginResponseDto>.Success(response);
        }
    }
}