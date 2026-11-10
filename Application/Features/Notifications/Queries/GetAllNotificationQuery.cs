
using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Notifications.Queries
{
    public sealed record GetAllNotificationQuery : IRequest<ResultWithValue<IReadOnlyCollection<NotificationResponseDto>>>;
}
