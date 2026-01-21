using Domain.Models;

namespace Application.Interface
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken(Guid userId);
    }
}