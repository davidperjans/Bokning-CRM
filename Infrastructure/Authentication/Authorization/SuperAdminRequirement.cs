using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication.Authorization
{
    public class SuperAdminRequirement : IAuthorizationRequirement
    {
    }
}