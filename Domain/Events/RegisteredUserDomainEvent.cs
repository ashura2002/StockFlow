namespace Domain.Events
{
    public sealed record RegisteredUserDomainEvent(Guid UserId, string Email) : IDomainEventMarker;
}
