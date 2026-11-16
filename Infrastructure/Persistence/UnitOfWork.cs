using Application.Interfaces;
using CloudinaryDotNet.Core;
using Domain.Entities;
using Domain.Events;
using Infrastructure.Data;



namespace Infrastructure.Persistence
{
    public sealed class UnitOfWork:IUnitOfWork
    {
        private readonly InventoryDbContext _context;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public UnitOfWork(InventoryDbContext inventoryDbContext, IDomainEventDispatcher domainEventDispatcher)
        {
            _context = inventoryDbContext;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            // every SaveChanges enters the loop and stops when there are no domain events
            while (true)
            {
                var domainEvents = CollectAndClearEvents();

                // stop processing when there are no domain events left
                if (domainEvents.Count == 0)
                    break;

                await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }



        private List<IDomainEventMarker> CollectAndClearEvents()
        {
            // get the domainevents in the original entity 
            var aggregates = _context.ChangeTracker.Entries<AggregateRoot>()
                .Select(entry => entry.Entity)
                .Where(entry => entry.DomainEvents.Count > 0)
                .ToList();

            // copy and stored the events to the new variable
            var domainEvents = aggregates
                .SelectMany(a => a.DomainEvents)
                .ToList();

            // clear all events in the original entity
            foreach (var item in aggregates)
            {
                item.ClearEvents();
            }

            return domainEvents;
        }
    }
}
