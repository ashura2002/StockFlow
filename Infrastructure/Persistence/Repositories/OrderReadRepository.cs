using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class OrderReadRepository : IOrderReadRepository
    {
        private readonly InventoryDbContext _context;

        public OrderReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<CustomerOrderResponseDto>> GetAllMyOrdersAsync(
            int page, 
            int pageSize, 
            Guid userId, 
            CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .Where(o => o.UserId == userId)
                .Select(o => new CustomerOrderResponseDto(
                    o.Id,
                    o.OrderItems.Select(item =>
                    new OrderItemResponse(
                        item.Id,
                        item.ProductId,
                        item.Product.ProductName.Value,
                        item.Quantity,
                        item.UnitPrice))
                    .ToList(),
                    o.OrderItems.Sum(i => 
                    i.UnitPrice * i.Quantity),
                    o.Status,
                    o.CreatedAt))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyCollection<AdminOrderResponseDto>> GetAllPendingOrdersAsync(
            int page, 
            int pageSize,
            CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .Where(o => o.Status == OrderStatus.Pending)
                .Select(o =>
                new AdminOrderResponseDto(
                    o.Id,
                    o.User.Email.Value,
                    o.OrderItems.Select(item =>
                    new OrderItemResponse(
                        item.Id, 
                        item.ProductId,
                        item.Product.ProductName.Value,
                        item.Quantity, 
                        item.UnitPrice))
                    .ToList(),
                    o.OrderItems.Sum(item =>
                    item.UnitPrice * item.Quantity),
                    o.Status,
                    o.CreatedAt)
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<CustomerOrderResponseDto?> GetMyOrderByIdAsync(
            Guid orderId, 
            Guid userId, 
            CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId && o.Id == orderId)
                .Select(o => new CustomerOrderResponseDto(
                    o.Id,
                    o.OrderItems.Select(item =>
                    new OrderItemResponse(
                        item.Id,
                        item.ProductId,
                        item.Product.ProductName.Value,
                        item.Quantity,
                        item.UnitPrice))
                    .ToList(),
                    o.OrderItems.Sum(i =>
                    i.UnitPrice * i.Quantity),
                    o.Status,
                    o.CreatedAt))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<AdminOrderResponseDto?> GetOrderByIdAsync(Guid orderId, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new AdminOrderResponseDto(
                    o.Id,
                    o.User.Email.Value,
                    o.OrderItems
                        .Select(item => new OrderItemResponse(
                            item.Id,
                            item.ProductId,
                            item.Product.ProductName.Value,
                            item.Quantity,
                            item.UnitPrice))
                        .ToList(),
                    // project the calculation because EF Core needs to translate
                    // the expression into SQL. The domain's computed TotalPrice
                    // property is C# logic and may not be translatable by EF Core.
                    o.OrderItems.Sum(item => item.UnitPrice * item.Quantity),
                    o.Status,
                    o.CreatedAt))
                .FirstOrDefaultAsync(ct);
        }
    }
}
