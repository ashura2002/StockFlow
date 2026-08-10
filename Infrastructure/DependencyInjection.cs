using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // registering and configurating database connection string
            services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


            // repositories
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // seeded
            services.AddScoped<DatabaseSeeder>();

            // services
            services.Configure<SeededUserSettings>(configuration.GetSection(SeededUserSettings.SectionName));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IJwtService, JwtService>();
            return services;
        }
    }
}
