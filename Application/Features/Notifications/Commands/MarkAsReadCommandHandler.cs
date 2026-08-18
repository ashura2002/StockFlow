using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Notifications.Commands
{
    public sealed class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand>
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

        public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var notification = await _notificationWriteRepository.GetNotificationById(
                request.NotificationId,
                currentUserId,
                cancellationToken) ??
                throw new DomainNotFoundException("Notification not found");

            notification.MarkAsRead();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
