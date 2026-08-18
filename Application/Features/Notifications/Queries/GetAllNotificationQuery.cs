
using Application.Dtos;
using MediatR;

namespace Application.Features.Notifications.Queries
{
    public sealed record GetAllNotificationQuery : IRequest<IReadOnlyCollection<NotificationResponseDto>>;
}
