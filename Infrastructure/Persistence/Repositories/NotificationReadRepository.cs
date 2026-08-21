
using Application.Dtos;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class NotificationReadRepository : INotificationReadRepository
    {
        private readonly InventoryDbContext _context;

        public NotificationReadRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public async Task<IReadOnlyCollection<NotificationResponseDto>> GetAllNotificationsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationResponseDto(n.Id, n.UserId, n.Content, n.IsRead))
                .ToListAsync(cancellationToken);
        }
    }
}
