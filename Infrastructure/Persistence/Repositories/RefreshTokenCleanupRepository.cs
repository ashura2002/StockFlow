using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class RefreshTokenCleanupRepository : IRefreshTokenCleanupRepository
    {
        private readonly InventoryDbContext _context;

        public RefreshTokenCleanupRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task DeleteExpiredAndRevokedTokensAsync(CancellationToken cancellationToken)
        {
            await _context.RefreshTokens
                .Where(rt => rt.ExpiresAt <= DateTime.UtcNow || rt.RevokedAt.HasValue)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
