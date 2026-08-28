using Application.Events;
using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Get the compiled Application assembly (Application.dll)
            var assembly = typeof(DependencyInjection).Assembly;

            // for mediatR registration
            services.AddMediatR(config =>
            // Scan the assembly and automatically register all IRequestHandler implementations
            config.RegisterServicesFromAssembly(assembly)
            );


            // events
            services.AddScoped<IDomainEventHandler<RegisteredUserDomainEvent>, RegisterUserDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<OrderCreatedDomainEvent>, OrderCreateDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<OrderConfirmedDomainEvent>, OrderConfirmDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<OrderCancelledDomainEvent>, OrderCancelDomainEventHandler>();
            services.AddScoped<IDomainEventHandler<OrderCompletedDomainEvent>, OrderCompleteDomainEventHandler>();
            return services;
        }
    }
}
