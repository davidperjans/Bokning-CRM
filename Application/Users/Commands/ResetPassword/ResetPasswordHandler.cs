using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Users.Commands.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        
        public ResetPasswordHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<bool>> Handle(ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByResetTokenAsync(request.Token, cancellationToken);
            
            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
            {
                return OperationResult<bool>.Failure("Invalid or expired reset token.");
            }
            
            user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            
            user.ResetPasswordToken = null;
            user.ResetTokenExpires = null;
            
            await _userRepository.UpdateUserAsync(user, cancellationToken);
            
            return OperationResult<bool>.Success(true);
        }
    }
}