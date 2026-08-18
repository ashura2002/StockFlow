using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly InventoryDbContext _context;

        public PasswordResetTokenRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public void Add(PasswordResetToken passwordResetToken)
        {
            _context.PasswordResetTokens.Add(passwordResetToken);
        }

        public async Task DeleteExpiredAndUsedTokenAsync(CancellationToken cancellationToken)
        {
            await _context.PasswordResetTokens
                .Where(t => t.ExpiresAt <= DateTime.UtcNow || t.UsedAt != null)
                .ExecuteDeleteAsync(cancellationToken); // directly translates the operation into a database DELETE query.
        }

        public async Task<PasswordResetToken?> GetByTokenHashAsync(string hashToken, CancellationToken cancellationToken)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(x => x.TokenHash == hashToken, cancellationToken);
        }
    }
}
