using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;

namespace Application.Events
{
    public sealed class RegisterUserDomainEventHandler : IDomainEventHandler<RegisteredUserDomainEvent>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserDomainEventHandler(
            INotificationWriteRepository notificationWriteRepository,
            IUserReadRepository userReadRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _userReadRepository = userReadRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task Handle(RegisteredUserDomainEvent domainEvent, CancellationToken ct)
        {
            var admin = await _userReadRepository.GetAdminAsync(ct) ??
                throw new DomainNotFoundException("System admin is missing. Please verify database seeding.");

            var notification = Notification.Create(admin.UserId, $"{domainEvent.Email} just registered a new account.");
            _notificationWriteRepository.Add(notification);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
