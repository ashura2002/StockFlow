using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Notifications.Queries
{
    public sealed class GetAllNotificationQueryHandler : IRequestHandler<GetAllNotificationQuery, ResultWithValue<IReadOnlyCollection<NotificationResponseDto>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationReadRepository _notificationReadRepository;

        public GetAllNotificationQueryHandler(ICurrentUserService currentUserService, INotificationReadRepository notificationReadRepository)
        {
            _currentUserService = currentUserService;
            _notificationReadRepository = notificationReadRepository;
        }

        public async Task<ResultWithValue<IReadOnlyCollection<NotificationResponseDto>>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var result = await _notificationReadRepository.GetAllNotificationsAsync(currentUserId, cancellationToken);
            return ResultWithValue<IReadOnlyCollection<NotificationResponseDto>>.Success(result);
        }
    }
}
