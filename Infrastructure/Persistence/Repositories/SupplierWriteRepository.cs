using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class SupplierWriteRepository : ISupplierWriteRepository
    {
        private readonly InventoryDbContext _context;

        public SupplierWriteRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public void Add(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
        }

        public async Task<Supplier?> GetSupplierByIdAsync(Guid supplierId, CancellationToken ct)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplierId, ct);
        }

        public void Remove(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }
    }
}
