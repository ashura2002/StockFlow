using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Notifications.Queries
{
    public sealed class GetAllNotificationQueryHandler : IRequestHandler<GetAllNotificationQuery, IReadOnlyCollection<NotificationResponseDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationReadRepository _notificationReadRepository;

        public GetAllNotificationQueryHandler(ICurrentUserService currentUserService, INotificationReadRepository notificationReadRepository)
        {
            _currentUserService = currentUserService;
            _notificationReadRepository = notificationReadRepository;
        }

        public async Task<IReadOnlyCollection<NotificationResponseDto>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            return await _notificationReadRepository.GetAllNotificationsAsync(currentUserId, cancellationToken);
        }
    }
}
