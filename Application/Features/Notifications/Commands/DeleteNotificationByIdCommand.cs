using MediatR;

namespace Application.Features.Notifications.Commands
{
    public sealed record DeleteNotificationByIdCommand(Guid NotificationId) : IRequest;
}
