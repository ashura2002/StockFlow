using Application.Dtos;
using Application.Interfaces;
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
                    s.CategoryName, 
                    s.Description))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsCategoryExistAsync(Guid CategoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .AnyAsync(c => c.Id == CategoryId, cancellationToken);
        }

        public async Task<bool> IsCategoryNameExistAsync(string CategoryName, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .AnyAsync(c => c.CategoryName == CategoryName, cancellationToken);
        }
    }
}
