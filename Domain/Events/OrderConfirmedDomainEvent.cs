namespace Domain.Events
{
    public sealed record OrderConfirmedDomainEvent(Guid OrderId, Guid UserId) : IDomainEventMarker;
}
