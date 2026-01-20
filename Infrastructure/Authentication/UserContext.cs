using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.Interface;

namespace Infrastructure.Authentication
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity!.IsAuthenticated)
                {
                    return null;
                }

                var userIdValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                  ?? user.FindFirst("sub")?.Value;

                if (Guid.TryParse(userIdValue, out var guid))
                {
                    return guid;
                }

                return null;
            }
        }
    }
}