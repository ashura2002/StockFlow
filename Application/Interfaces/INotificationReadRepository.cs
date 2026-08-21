using Application.Dtos;

namespace Application.Interfaces
{
    public interface INotificationReadRepository
    {
        Task<IReadOnlyCollection<NotificationResponseDto>> GetAllNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
