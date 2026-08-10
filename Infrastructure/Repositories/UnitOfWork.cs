using Application.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        public InventoryDbContext _context;

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
