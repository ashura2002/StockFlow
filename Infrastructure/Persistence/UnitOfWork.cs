using Application.Interfaces;
using Domain.Entities;
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
            await _context.SaveChangesAsync(cancellationToken);

            // Get all tracked aggregate roots.
            var aggregateEntries = _context.ChangeTracker
                .Entries<AggregateRoot>();

            // Copy all domain events from the aggregates into a separate list.
            // This allows us to safely clear the events before dispatching them.
            var domainEvents = aggregateEntries
                .SelectMany(entry => entry.Entity.DomainEvents)
                .ToList();

            // Clear dispatched events to prevent duplicate dispatching.
            foreach (var entry in aggregateEntries)
            {
                entry.Entity.ClearEvents();
            }

            if (domainEvents.Count > 0)
            {
                // Dispatch all collected domain events.
                await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
            }
        }
    }
}
