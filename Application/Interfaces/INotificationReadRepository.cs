using Application.Dtos;

namespace Application.Interfaces
{
    public interface INotificationReadRepository
    {
        Task<IReadOnlyCollection<NotificationResponseDto>> GetAllNotifications(Guid userId, CancellationToken cancellationToken);
    }
}
