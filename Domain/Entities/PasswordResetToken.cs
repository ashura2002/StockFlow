using Domain.Exceptions;

namespace Domain.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;


        public string TokenHash { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? UsedAt { get; private set; }

        public bool IsUsed => UsedAt.HasValue;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        private PasswordResetToken(
            Guid userId,
            string tokenHash,
            DateTime expiresAt)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
        }

        public static PasswordResetToken Create(
            Guid userId, 
            string tokenHash, 
            DateTime expiresAt)
        {
            if (userId == Guid.Empty) 
                throw new DomainBadRequestException("User Id is required");

            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new DomainBadRequestException("Token hash is required.");

            if (expiresAt <= DateTime.UtcNow)
                throw new DomainBadRequestException(
                    "Token expiration must be in the future.");

            return new PasswordResetToken(userId, tokenHash, expiresAt);
        }

        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new DomainBadRequestException(
                    "Password reset token has already been used.");

            if (IsExpired)
                throw new DomainBadRequestException(
                    "Password reset token has expired.");

            UsedAt = DateTime.UtcNow;
            Touch();
        }
    }
}
