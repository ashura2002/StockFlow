using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class ProductWriteRepository : IProductWriteRepository
    {
        private readonly InventoryDbContext _context;

        public ProductWriteRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId, ct);
        }
    }
}
