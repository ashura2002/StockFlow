using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        void Add(PasswordResetToken passwordResetToken);
        Task<PasswordResetToken?> GetByTokenHashAsync(string hashToken, CancellationToken cancellationToken);
    }
}
