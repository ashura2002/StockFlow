namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User? User { get; private set; }
        public string TokenHash { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }



        private RefreshToken(
            Guid userId,
            string tokenHash,
            DateTime expiresAt)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
        }

        public static RefreshToken Create(
            Guid userId,
            string tokenHash,
            DateTime expiresAt)
        {
            return new RefreshToken(userId, tokenHash, expiresAt);
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= ExpiresAt;
        }

        public bool IsRevoked()
        {
            return RevokedAt.HasValue;
        }

        public bool IsActive()
        {
            return !IsExpired() && !IsRevoked();
        }

        public void Revoke()
        {
            if (IsRevoked())
                return;

            RevokedAt = DateTime.UtcNow;
            Touch();
        }
    }
}
