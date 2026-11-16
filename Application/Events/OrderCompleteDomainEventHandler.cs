using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;

namespace Application.Events
{
    public sealed class OrderCompleteDomainEventHandler : IDomainEventHandler<OrderCompletedDomainEvent>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUserReadRepository _userReadRepository;

        public OrderCompleteDomainEventHandler(
            INotificationWriteRepository notificationWriteRepository,
            IUserReadRepository userReadRepository)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task Handle(OrderCompletedDomainEvent domainEvent, CancellationToken ct)
        {
            var user = await _userReadRepository.GetUserByIdAsync(domainEvent.UserId, ct) ??
                throw new DomainNotFoundException("User not found");

            var notification = Notification.Create(user.UserId,
                "Your order has been completed successfully.");
            _notificationWriteRepository.Add(notification);
        }
    }
}
