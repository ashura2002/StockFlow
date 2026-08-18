
using Domain.Events;

namespace Application.Interfaces
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent: IDomainEventMarker
    {
        Task Handle(TDomainEvent domainEvent, CancellationToken ct);
    }
}
