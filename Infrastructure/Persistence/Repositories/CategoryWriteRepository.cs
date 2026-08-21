using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class CategoryWriteRepository : ICategoryWriteRepository
    {
        private readonly InventoryDbContext _context;

        public CategoryWriteRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId, CancellationToken ct)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId, ct);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }
    }
}
