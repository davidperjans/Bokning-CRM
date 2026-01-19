using Application.Interface;

namespace Infrastructure.Authentication
{
    public class PasswordHasher :IPasswordHasher
    {
        public string HashPassword(string password) => 
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool VerifyPassword(string password, string hash) => 
            BCrypt.Net.BCrypt.EnhancedVerify(password, hash);   
    }
}