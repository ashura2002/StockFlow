using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICategoryWriteRepository
    {
        void Add(Category category);
        void Remove(Category category);
        Task<Category?> GetCategoryByIdAsync(Guid categoryId, CancellationToken ct);
    }
}
