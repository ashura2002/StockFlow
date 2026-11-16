using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace Application.Events
{
    public sealed class RegisterUserDomainEventHandler : IDomainEventHandler<RegisteredUserDomainEvent>
    {
        private readonly INotificationWriteRepository _notificationWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ILogger<RegisteredUserDomainEvent> _logger;

        public RegisterUserDomainEventHandler(
            INotificationWriteRepository notificationWriteRepository,
            IUserReadRepository userReadRepository,
            ILogger<RegisteredUserDomainEvent> logger)
        {
            _notificationWriteRepository = notificationWriteRepository;
            _userReadRepository = userReadRepository;
            _logger = logger;
        }


        public async Task Handle(RegisteredUserDomainEvent domainEvent, CancellationToken ct)
        {
            var admin = await _userReadRepository.GetAdminAsync(ct) ??
                throw new DomainNotFoundException("System admin is missing. Please verify database seeding.");

            var notification = Notification.Create(admin.UserId, $"{domainEvent.Email} just registered a new account.");
            _logger.LogInformation("New user created {UserId}", domainEvent.UserId);
            _notificationWriteRepository.Add(notification);
        }
    }
}
