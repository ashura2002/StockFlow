using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISupplierWriteRepository
    {

        void Add(Supplier supplier);
        void Remove(Supplier supplier);
        Task<Supplier?> GetSupplierByIdAsync(Guid supplierId, CancellationToken ct);

    }
}
