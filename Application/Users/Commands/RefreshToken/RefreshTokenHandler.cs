using Application.Common;
using Application.Interface;
using Application.Users.Dtos;
using MediatR;
using Domain.Models;

namespace Application.Users.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, OperationResult<LoginResponseDto>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        
        public RefreshTokenHandler(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<OperationResult<LoginResponseDto>> Handle(RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var storedToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);
            
            if (storedToken == null || storedToken.Revoked || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return OperationResult<LoginResponseDto>.Failure("Invalid or expired refresh token.");
            }
            
            storedToken.Revoked = true;
            await _userRepository.UpdateRefreshTokenAsync(storedToken, cancellationToken);
            
            var newJwtToken = _tokenService.GenerateJwtToken(storedToken.User);
            var newRefreshTokenString = _tokenService.GenerateRefreshToken(storedToken.UserId);
            var expiryDate = DateTime.UtcNow.AddDays(7);
            
            var newRefreshToken = new Domain.Models.RefreshToken
            {
                Token = newRefreshTokenString,
                UserId = storedToken.UserId,
                ExpiresAt = expiryDate,
                CreatedAt = DateTime.UtcNow
            };
            
            await _userRepository.SaveRefreshTokenAsync(newRefreshToken, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            // 5. Mappa User till UserDto och returnera LoginResponseDto
            var userDto = new UserDto(
                storedToken.User.Id,
                storedToken.User.Email,
                storedToken.User.FirstName,
                storedToken.User.LastName,
                storedToken.User.Phone,
                storedToken.User.DateOfBirth,
                storedToken.User.Role.ToString(), // Om rollen är en Enum, konvertera till string
                storedToken.User.CreatedAt
            );
            return OperationResult<LoginResponseDto>.Success(new LoginResponseDto(newJwtToken, newRefreshTokenString, expiryDate, userDto));
        }
    }
}