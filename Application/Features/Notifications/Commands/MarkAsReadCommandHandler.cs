using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Notifications.Commands
{
    public sealed class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, Result>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsReadCommandHandler(
            INotificationWriteRepository notificationWriteRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var notification = await _notificationWriteRepository.GetNotificationByIdAsync(
                request.NotificationId,
                currentUserId,
                cancellationToken);

            if (notification is null)
                return Result.Failure(NotificationErrors.NotificationNotFound);

            notification.MarkAsRead();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
