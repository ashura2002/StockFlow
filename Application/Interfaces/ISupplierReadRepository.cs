
using Application.Dtos;
using Domain.ValueObjects;

namespace Application.Interfaces
{
    public interface ISupplierReadRepository
    {
        Task<bool> IsSupplierEmailExistAsync(string email, Guid? excludingSupplierId, CancellationToken ct);
        Task<bool> IsSupplierExistAsync(Guid supplierId, CancellationToken ct);
        Task<IReadOnlyCollection<SupplierResponseDto>> GetAllSuppliersAsync(int page, int pageSize, CancellationToken ct);
    }
}
