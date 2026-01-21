using System.Security.Cryptography;
using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Users.Commands.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, OperationResult<bool>>
    {
        private readonly IUserRepository _repository;
        
        public ForgotPasswordHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<bool>> Handle(ForgotPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return OperationResult<bool>.Success(true);
            }

            var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            
            user.ResetPasswordToken = resetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
            
            await _repository.SaveChangesAsync(cancellationToken);
            
               // TODO: Här ska IEmailService anropas i framtiden.
            // Just nu lägger vi t.ex. logga token till konsolen för att kunna testa:
            
            Console.WriteLine($"DEBUG:Reset token for {user.Email}: {resetToken}");
            
            return OperationResult<bool>.Success(true);
        }
    }
}