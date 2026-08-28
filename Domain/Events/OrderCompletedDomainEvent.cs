namespace Domain.Events
{
    public sealed record OrderCompletedDomainEvent(Guid OrderId, Guid UserId) : IDomainEventMarker;
}
