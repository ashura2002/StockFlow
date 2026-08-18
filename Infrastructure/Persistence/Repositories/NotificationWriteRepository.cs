using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class NotificationWriteRepository : INotificationWriteRepository
    {
        private readonly InventoryDbContext _context;

        public NotificationWriteRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public void Add(Notification notification)
        {
            _context.Add(notification);
        }

        public async Task<Notification?> GetNotificationById(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && 
                    n.UserId == userId, cancellationToken);
        }

        public void Remove(Notification notification)
        {
            _context.Remove(notification);
        }
    }
}
