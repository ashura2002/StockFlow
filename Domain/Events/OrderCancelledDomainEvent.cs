using Domain.Enums;

namespace Domain.Events
{
    public sealed record OrderCancelledDomainEvent(
        Guid OrderId,
        Guid UserId,
        OrderCancellationSource Source) : IDomainEventMarker;
}
