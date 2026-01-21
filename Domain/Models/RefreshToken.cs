 namespace Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Revoked { get; set; }
        
        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive() => !Revoked && !IsExpired();
    }
}