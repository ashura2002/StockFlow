using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;

namespace Application.Events
{
    public sealed class OrderCancelDomainEventHandler : IDomainEventHandler<OrderCancelledDomainEvent>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderCancelDomainEventHandler(
            INotificationWriteRepository notificationWriteRepository,
            IUserReadRepository userReadRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _userReadRepository = userReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken ct)
        {
            Guid recipientId;
            string message;

            if (domainEvent.Source == OrderCancellationSource.Customer)
            {
                // if customer cancelled the order, admin will notify
                var admin = await _userReadRepository.GetAdminAsync(ct)
                    ?? throw new DomainNotFoundException(
                    "System admin is missing. Please verify database seeding.");

                recipientId = admin.UserId;
                message = "A customer has cancelled an order.";
            }
            else
            {
                // admin cancelled the order, so notify the customer who owns the order.
                recipientId = domainEvent.UserId;
                message = "Admin cancelled your order.";
            }

            var notification = Notification.Create(recipientId, message);
            _notificationWriteRepository.Add(notification);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
