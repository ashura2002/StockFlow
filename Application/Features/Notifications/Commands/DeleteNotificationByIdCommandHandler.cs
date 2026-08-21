using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Notifications.Commands
{
    public sealed class DeleteNotificationByIdCommandHandler : IRequestHandler<DeleteNotificationByIdCommand>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteNotificationByIdCommandHandler(
            INotificationWriteRepository notificationWriteRepository, 
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteNotificationByIdCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var notification = await _notificationWriteRepository.GetNotificationByIdAsync(request.NotificationId, currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("Notification not found");
            _notificationWriteRepository.Remove(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
