
namespace Application.Interfaces
{
    public interface IRefreshTokenCleanupRepository
    {
        Task DeleteExpiredAndRevokedTokensAsync(CancellationToken cancellationToken);
    }
}
