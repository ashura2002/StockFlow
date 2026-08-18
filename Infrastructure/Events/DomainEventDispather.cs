using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Events
{
    public sealed class DomainEventDispather : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispather(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task DispatchAsync(IEnumerable<IDomainEventMarker> domainEvents, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in domainEvents)
            {
                // get the actual runtime type of the event
                // ex output: RegisteredUserDomainEvent
                var eventType = domainEvent.GetType();

                // construct the correspondng handler type
                // ex; IDomainHandler<RegisteredUserDomainEvent>
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

                // get all the registerd handlers for this event type in DI container
                var handlers = _serviceProvider.GetServices(handlerType);

                // loop the handlers and invoke every handler registered
                foreach (var handle in handlers)
                {
                    // get the runtime type of the handler
                    var handleRuntimeType = handle!.GetType();

                    // get the method using reflection
                    var runtimeMethod = handleRuntimeType.GetMethod(nameof(IDomainEventHandler<IDomainEventMarker>.Handle));

                    // invoke the handle (Handle(domainEvent, cancellationToken))
                    var result = runtimeMethod?.Invoke(handle, new object[] { domainEvent, cancellationToken });

                    // await the handler if it retrn a Task
                    if (result is Task task) await task;
                }
            }
        }
    }
}