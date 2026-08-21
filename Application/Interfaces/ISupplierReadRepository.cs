
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ISupplierReadRepository
    {
        Task<bool> IsSupplierEmailExistAsync(string email, CancellationToken ct);
        Task<bool> IsSupplierExistAsync(Guid supplierId, CancellationToken ct);
        Task<IReadOnlyCollection<SupplierResponseDto>> GetAllSuppliersAsync(int page, int pageSize, CancellationToken ct);
    }
}
