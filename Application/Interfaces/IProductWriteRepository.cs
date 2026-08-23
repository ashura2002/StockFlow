using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductWriteRepository
    {
        void Add(Product product);
        Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken ct);

    }
}
