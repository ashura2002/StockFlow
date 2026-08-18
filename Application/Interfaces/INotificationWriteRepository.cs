
using Domain.Entities;

namespace Application.Interfaces
{
    public interface INotificationWriteRepository
    {
        void Add(Notification notification);
        void Remove(Notification notification);
        Task<Notification?> GetNotificationById(Guid notificationId, Guid userId, CancellationToken cancellationToken);
    }
}
