using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;

namespace Application.Events
{
    public sealed class OrderConfirmDomainEventHandler : IDomainEventHandler<OrderConfirmedDomainEvent>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUserReadRepository _userReadRepository;

        public OrderConfirmDomainEventHandler(
            INotificationWriteRepository notificationWriteRepository,
            IUserReadRepository userReadRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task Handle(OrderConfirmedDomainEvent domainEvent, CancellationToken ct)
        {
            var user = await _userReadRepository.GetUserByIdAsync(domainEvent.UserId, ct) ??
                throw new DomainNotFoundException("User not found.");

            var notification = Notification.Create(user.UserId, "Your order has been confirmed and is now being processed.");
            _notificationWriteRepository.Add(notification);
        }
    }
}
