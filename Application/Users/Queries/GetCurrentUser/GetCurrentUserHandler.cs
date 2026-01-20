using Application.Common;
using Application.Interface;
using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, OperationResult<UserDto>>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _repository;
        
        public GetCurrentUserHandler(IUserContext userContext, IUserRepository repository)
        {
            _userContext = userContext;
            _repository = repository;
        }

        public async Task<OperationResult<UserDto>> Handle(GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            
            if (userId == null)
            {
                return OperationResult<UserDto>.Failure("Användaren är inte inloggad.");
            }
            
            var user = await _repository.GetUserByIdAsync(userId.Value);
            
            if (user == null)
            {
                return OperationResult<UserDto>.Failure("Användaren hittades inte.");
            }
            
            var userDto = new UserDto
            (
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