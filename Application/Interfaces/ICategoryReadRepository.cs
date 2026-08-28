
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICategoryReadRepository
    {
        Task<IReadOnlyCollection<CategoryResponseDto>> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task<bool> IsCategoryNameExistAsync(string categoryName, Guid? excludingCategoryId, CancellationToken cancellationToken);
        Task<bool> IsCategoryExistAsync(Guid categoryId, CancellationToken cancellationToken);
    }
}
