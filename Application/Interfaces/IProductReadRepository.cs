using Application.Dtos;

namespace Application.Interfaces
{
    public interface IProductReadRepository
    {
        Task<bool> IsProductNameExistAsync(string productName, Guid? productId, CancellationToken ct);
        Task<IReadOnlyCollection<ProductResponseDto>> GetAllProductsAsync(int page, int pageSize, CancellationToken ct);
        Task<ProductResponseDto?> GetProductByIdAsync(Guid productId, CancellationToken ct);
        Task<IReadOnlyCollection<ProductResponseDto>> SearchProductsByNameAsync(string productName, int page, int pageSize, CancellationToken ct);
        Task<IReadOnlyCollection<DeletedProductResponseDto>> GetAllDeletedProducts(CancellationToken ct);
    }
}
