using Application.Interfaces;
using Infrastructure.Data;


namespace Infrastructure.Persistence
{
    public sealed class UnitOfWork:IUnitOfWork
    {
        private readonly InventoryDbContext _context;

        public UnitOfWork(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
