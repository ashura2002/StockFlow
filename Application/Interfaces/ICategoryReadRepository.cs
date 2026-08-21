
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ICategoryReadRepository
    {
        Task<IReadOnlyCollection<CategoryResponseDto>> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task<bool> IsCategoryNameExistAsync(string CategoryName, CancellationToken cancellationToken);
        Task<bool> IsCategoryExistAsync(Guid CategoryId, CancellationToken cancellationToken);
    }
}
