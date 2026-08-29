namespace WebAPI
{
    public static class CorsDependencyInjection
    {

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(option => option.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }));

            return services;
        }
    }
}
