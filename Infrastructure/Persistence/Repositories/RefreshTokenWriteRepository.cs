
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class RefreshTokenWriteRepository : IRefreshTokenWriteRepository
    {
        private readonly InventoryDbContext _context;

        public RefreshTokenWriteRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public void Add(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }
    }
}
