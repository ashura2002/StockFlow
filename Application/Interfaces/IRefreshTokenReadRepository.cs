using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenReadRepository
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);
    }
}
