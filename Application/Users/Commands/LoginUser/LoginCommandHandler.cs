using Application.Common;
using Application.Interface;
using Application.Users.Dtos;
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
          if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash)) // Verify password
          {
              return OperationResult<LoginResponseDto>.Failure("Invalid email or password.");
          }
          
          var token = _tokenService.GenerateJwtToken(user);
          
          var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Phone, user.DateOfBirth, user.Role.ToString(), user.CreatedAt);
          
          var response = new LoginResponseDto(
              token, DateTime.Now, userDto);
          
          return OperationResult<LoginResponseDto>.Success(response);
        
      }
    }
}