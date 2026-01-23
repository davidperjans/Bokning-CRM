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

                // I UserContext.cs
                var userIdValue = user.FindFirst("sub")?.Value 
                                  ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Guid.TryParse(userIdValue, out var guid))
                {
                    return guid;
                }

                return null;
            }
        }

        public Guid? BusinessId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity!.IsAuthenticated)
                {
                    return null;
                }

                // Försök hitta BusinessId-claim
                // Byt claim-namn här om du använder något annat (t.ex. "businessId" eller "business_id")
                var businessIdValue =
                    user.FindFirst("businessId")?.Value
                    ?? user.FindFirst("business_id")?.Value
                    ?? user.FindFirst("business")?.Value;

                if (Guid.TryParse(businessIdValue, out var guid))
                {
                    return guid;
                }

                return null;
            }
        }

        public string? Role
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                // Vi letar efter "role" direkt eftersom vi rensat standard-mappningen
                return user?.FindFirst("role")?.Value;
            }
        }

    }
}