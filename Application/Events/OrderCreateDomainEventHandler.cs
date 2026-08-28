using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;

namespace Application.Events
{
    public sealed class OrderCreateDomainEventHandler : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderCreateDomainEventHandler(
            INotificationWriteRepository notificationWriteRepository,
            IUserReadRepository userReadRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _userReadRepository = userReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken ct)
        {
            var user = await _userReadRepository.GetAdminAsync(ct)??
                throw new DomainNotFoundException("System admin is missing. Please verify database seeding.");

            var notification = Notification.Create(user.UserId,
                $"A new customer order has been created and is awaiting confirmation.");
            _notificationWriteRepository.Add(notification);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
