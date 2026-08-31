using Application.Dtos;
using Application.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly InventoryDbContext _context;
        public CategoryReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<CategoryResponseDto>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .Select(s =>
                new CategoryResponseDto(
                    s.Id,
                    s.CategoryName.Value,
                    s.Description))
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoryWithProductsResponseDto?> GetCategoryByIdWithProductsAsync(
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == categoryId)
                .Select(c => 
                new CategoryWithProductsResponseDto(
                    c.Id, 
                    c.CategoryName.Value, 
                    c.Description,
                c.Products.Select(p =>
                new ProductResponseDto(
                    p.Id,
                    p.ProductName.Value,
                    p.Price,
                    p.Stock,
                    p.Category.CategoryName.Value,
                    p.Supplier.Name,
                    p.ProductDescriptions,
                    p.ProductImageUrl,
                    p.ProductImagePublicId)).ToList()))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsCategoryExistAsync(Guid CategoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .AnyAsync(c => c.Id == CategoryId, cancellationToken);
        }

        public async Task<bool> IsCategoryNameExistAsync(string CategoryName, Guid? excludingCategoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .AnyAsync(c => 
                c.CategoryName == CategoryNameVo.Create(CategoryName) &&
                (excludingCategoryId == null || c.Id != excludingCategoryId),
                cancellationToken);
        }
    }
}
