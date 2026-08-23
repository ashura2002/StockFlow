using Application.Dtos;
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

        public async Task<IReadOnlyCollection<DeletedProductResponseDto>> GetAllDeletedProducts(CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .IgnoreQueryFilters()
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.DeletedAt.HasValue)
                .Select(p => new DeletedProductResponseDto(
                          p.Id,
                          p.ProductName.Value,
                          p.Price,
                          p.Stock,
                          p.Category.CategoryName,
                          p.Supplier.Name,
                          p.ProductDescriptions,
                          p.ProductImageUrl,
                          p.ProductImagePublicId,
                          p.DeletedAt))
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyCollection<ProductResponseDto>> GetAllProductsAsync(int page, int pageSize, CancellationToken ct)
        {
            return await _context.Products
              .AsNoTracking()
              .OrderByDescending(p => p.CreatedAt)
              .Select(p => new ProductResponseDto(
                          p.Id,
                          p.ProductName.Value,
                          p.Price,
                          p.Stock,
                          p.Category.CategoryName,
                          p.Supplier.Name,
                          p.ProductDescriptions,
                          p.ProductImageUrl,
                          p.ProductImagePublicId))
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync(ct);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid productId, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new ProductResponseDto(
                          p.Id,
                          p.ProductName.Value,
                          p.Price,
                          p.Stock,
                          p.Category.CategoryName,
                          p.Supplier.Name,
                          p.ProductDescriptions,
                          p.ProductImageUrl,
                          p.ProductImagePublicId
                    ))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IReadOnlyCollection<ProductResponseDto>> SearchProductsByNameAsync(
            string productName,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            return await _context.Database.SqlQuery<ProductResponseDto>($"""
                SELECT 
                    p."Id" AS "ProductId",
                    p."ProductName" AS "ProductName",
                    p."Price" AS "Price",
                    p."Stock" AS "Stock",
                    c."CategoryName" AS "Category",
                    s."Name" AS "Supplier",
                    p."ProductDescriptions" AS "ProductDescriptions",
                    p."ProductImageUrl" AS "ProductImageUrl",
                    p."ProductImagePublicId" AS "ProductImagePublicId"
                FROM "Products" AS p
                INNER JOIN "Categories" AS c
                    ON p."CategoryId" = c."Id"
                INNER JOIN "Suppliers" AS s
                    ON p."SupplierId"= s."Id"
                WHERE p."DeletedAt" IS NULL AND
                p."ProductName" ILIKE '%' || {productName} || '%'
                ORDER BY p."CreatedAt" DESC
                LIMIT {pageSize}
                OFFSET {(page - 1) * pageSize}
                """).ToListAsync(ct);
        }

        public async Task<bool> IsProductNameExistAsync(string productName, Guid? productId, CancellationToken ct)
        {
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.ProductName == ProductNameVo.Create(productName)
                && (productId == null || p.Id != productId), ct);
        }
    }
}
