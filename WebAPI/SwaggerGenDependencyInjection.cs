using Microsoft.OpenApi;

namespace WebAPI
{
    public static class SwaggerGenDependencyInjection
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Inventory System",
                    Description = """
                        A RESTful API for managing products, categories, suppliers, users, and customer orders.

                        Built with ASP.NET Core using Clean Architecture, CQRS, DDD, SOLID principles,
                        and object-oriented design to promote separation of concerns, maintainability,
                        and clear domain boundaries.
                        """,
                    Version = "v1"
                });

                opt.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                opt.AddSecurityRequirement(docs => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", docs)] = new()
                });
            }); ;


            return services;
        }
    }
}