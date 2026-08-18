
using Domain.Events;

namespace Application.Interfaces
{
    public interface IDomainEventDispatcher
    {
        public Task DispatchAsync(IEnumerable<IDomainEventMarker> domainEvents, CancellationToken cancellationToken);
    }
}
