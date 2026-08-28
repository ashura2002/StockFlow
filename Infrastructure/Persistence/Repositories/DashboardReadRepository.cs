using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class DashboardReadRepository : IDashboardReadRepository
    {
        private readonly InventoryDbContext _context;
        public DashboardReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> GetDashboardAsync(CancellationToken ct)
        {
            var dashboard = await _context.Orders
              .AsNoTracking()
              .GroupBy(_ => 1)
              .Select(g => new DashboardResponseDto(
                  g.Where(o => o.Status == OrderStatus.Completed)
                      .Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)),

                  g.Count(),

                  g.Count(o => o.Status == OrderStatus.Completed),

                  g.Count(o => o.Status == OrderStatus.Pending),

                  g.Count(o => o.Status == OrderStatus.Cancelled),

                  g.Count(o => o.Status == OrderStatus.Confirmed)))
              .FirstOrDefaultAsync(ct);

            return dashboard ??
                new DashboardResponseDto(0, 0, 0, 0, 0, 0);
        }
    }
}
