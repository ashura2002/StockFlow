using Application.Dtos;
using Application.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class SupplierReadRepository : ISupplierReadRepository
    {
        private readonly InventoryDbContext _context;

        public SupplierReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<SupplierResponseDto>> GetAllSuppliersAsync(int page, int pageSize, CancellationToken ct)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new SupplierResponseDto(
                    s.Id,
                    s.Name,
                    s.Email.Value,
                    s.PhoneNumber.Value,
                    s.Address.Value))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<SupplierWithProductsResponseDto?> GetSupplierByIdWithProductsAsync(Guid supplierId, CancellationToken cancellationToken)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.Id == supplierId)
                .Include(s => s.Products)
                .Select(s =>
                new SupplierWithProductsResponseDto(
                    s.Id,
                    s.Name,
                    s.Email.Value,
                    s.PhoneNumber.Value,
                    s.Address.Value,
                    s.Products.Select(p =>
                    new ProductResponseDto(
                        p.Id,
                        p.ProductName.Value,
                        p.Price,
                        p.Stock,
                        p.Category.CategoryName.Value,
                        p.Supplier.Name,
                        p.ProductDescriptions,
                        p.ProductImageUrl,
                        p.ProductImagePublicId))
                    .ToList()))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsSupplierEmailExistAsync(string email, Guid? excludingSupplierId, CancellationToken ct)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .AnyAsync(s => s.Email == EmailVo.Create(email) &&
                    (excludingSupplierId == null || s.Id != excludingSupplierId), ct);
        }

        public async Task<bool> IsSupplierExistAsync(Guid supplierId, CancellationToken ct)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .AnyAsync(s => s.Id == supplierId, ct);
        }
    }
}
