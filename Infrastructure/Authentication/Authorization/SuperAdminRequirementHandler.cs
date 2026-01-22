using Application.Interface;
using Domain.Enum;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication.Authorization
{
    public class SuperAdminRequirementHandler : AuthorizationHandler<SuperAdminRequirement>
    {
        private readonly AppDbContext _context;
        private readonly IUserContext _userContext;
        
        public SuperAdminRequirementHandler(AppDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }
        
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SuperAdminRequirement requirement)
        {
            var userId = _userContext.UserId; // Kontrollera att denna hämtar från "sub"

            if (!userId.HasValue || userId.Value == Guid.Empty)
            {
                return;
            }
            
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId.Value)
                .Select(u => new { u.Role })
                .FirstOrDefaultAsync();

            if (user != null && user.Role == UserRole.SuperAdmin)
            {
                context.Succeed(requirement);
            }
        }
    }
}