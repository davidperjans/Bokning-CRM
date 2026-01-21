using Application.Interface;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken,
            CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token,
            CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
        }

        public Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            _context.RefreshTokens.Update(refreshToken);
            
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByResetTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var normalizedToken = token.Trim().ToUpper();
            
            
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ResetPasswordToken != null && 
                                          u.ResetPasswordToken.Trim().ToUpper() == normalizedToken, 
                    cancellationToken);
        }

        public  Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(user);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}