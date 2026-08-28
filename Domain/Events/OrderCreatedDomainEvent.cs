namespace Domain.Events
{
    public sealed record OrderCreatedDomainEvent(Guid OrderId, Guid UserId):IDomainEventMarker;
}
