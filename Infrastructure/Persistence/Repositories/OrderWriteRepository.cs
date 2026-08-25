using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class OrderWriteRepository : IOrderWriteRepository
    {
        private readonly InventoryDbContext _context;

        public OrderWriteRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken ct)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId, ct);
        }

        public void Remove(Order order)
        {
            _context.Remove(order);
        }
    }
}
