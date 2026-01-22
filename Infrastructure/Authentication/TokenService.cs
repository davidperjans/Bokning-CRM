using System.Security.Claims;
using System.Text;
using Application.Interface;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        
        public TokenService(IConfiguration configuration) => _configuration = configuration;

        public string GenerateJwtToken(User user)
        { 
            var jwtSettings = _configuration.GetSection("JwtSettings");
            
            // 1. Hämta och validera Secret
            var secret = jwtSettings["Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT Secret is missing in appsettings.json");
            }
            
            // Använd variabeln 'secret' som vi precis kontrollerade
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 2. Sätt upp Claims
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Använd gärna standardnamn
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("role", user.Role.ToString()), 
                new("name", user.FirstName)      
            };

            // 3. Hantera utgångstid säkert (Fallback till 60 minuter)
            if (!double.TryParse(jwtSettings["ExpiryMinutes"], out var expiryMinutes))
            {
                expiryMinutes = 60;
            }
            
            // 4. Skapa Token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes), 
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                // Konverterar arrayen till en läsbar Base64-sträng
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}