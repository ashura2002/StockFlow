using Application.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class ProductReadRepository : IProductReadRepository
    {
        private readonly InventoryDbContext _context;
        public ProductReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsProductNameExistAsync(string productName, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.ProductName == ProductNameVo.Create(productName), ct);
        }
    }
}
