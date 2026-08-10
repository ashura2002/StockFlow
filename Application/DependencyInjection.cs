using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Get the compiled Application assembly (Application.dll)
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(config =>
            // Scan the assembly and automatically register all IRequestHandler implementations
            config.RegisterServicesFromAssembly(assembly)
            );

            return services;
        }
    }
}
