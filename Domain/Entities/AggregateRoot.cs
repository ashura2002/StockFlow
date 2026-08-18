
using Domain.Events;

namespace Domain.Entities
{
    public class AggregateRoot:BaseEntity
    {
        private List<IDomainEventMarker> _domainEvents = new();

        public IReadOnlyCollection<IDomainEventMarker> DomainEvents => _domainEvents.AsReadOnly();


       protected void RaiseEvent(IDomainEventMarker domainEvent)
        {
            Console.WriteLine("Event raising...");
            _domainEvents.Add(domainEvent);
        }

        public void ClearEvents()
        {
            Console.WriteLine("Clearing events...");
            _domainEvents.Clear();
        }
    }
}
