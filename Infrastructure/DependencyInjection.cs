using Application.Interfaces;
using Infrastructure.BackgroundServices;
using Infrastructure.Data;
using Infrastructure.Events;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

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
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // seeded
            services.AddScoped<DatabaseSeeder>();

            // services
            services.Configure<SeededUserSettings>(configuration.GetSection(SeededUserSettings.SectionName));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

            // Resend external service
            services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = configuration["Email:ApiKey"]
                    ?? throw new InvalidOperationException(
                        "Resend API key is not configured.");
            });
            services.AddHttpClient<IResend, ResendClient>();

            // Background service
            services.AddHostedService<PasswordResetTokenCleanupService>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
            services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispather>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IPasswordTokenGeneratorService, PasswordTokenGeneratorService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IPasswordResetTokenHasherService, PasswordResetTokenHasherService>();
            return services;
        }
    }
}
