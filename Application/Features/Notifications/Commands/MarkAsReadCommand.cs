using MediatR;

namespace Application.Features.Notifications.Commands
{
    public sealed record MarkAsReadCommand(Guid NotificationId) : IRequest;
}
