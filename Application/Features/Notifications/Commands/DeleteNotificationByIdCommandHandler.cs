using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Notifications.Commands
{
    public sealed class DeleteNotificationByIdCommandHandler : IRequestHandler<DeleteNotificationByIdCommand, Result>
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

        public async Task<Result> Handle(DeleteNotificationByIdCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var notification = await _notificationWriteRepository.GetNotificationByIdAsync(request.NotificationId, currentUserId, cancellationToken);
            if (notification is null)
                return Result.Failure(NotificationErrors.NotificationNotFound);

            _notificationWriteRepository.Remove(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
