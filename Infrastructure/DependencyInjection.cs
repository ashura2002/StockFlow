using Application.Interfaces;
using CloudinaryDotNet;
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
using Microsoft.Extensions.Options;
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
            services.AddScoped<IProfileWriteRepository, ProfileWriteRepository>();
            services.AddScoped<IProfileReadRepository, ProfileReadRepository>();
            services.AddScoped<ISupplierWriteRepository, SupplierWriteRepository>();
            services.AddScoped<ISupplierReadRepository, SupplierReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            

            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // seeded
            services.AddScoped<DatabaseSeeder>();

            // configs
            services.Configure<SeededUserSettings>(configuration.GetSection(SeededUserSettings.SectionName));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
            services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SectionName));

            // external services
            // Resend
            services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = configuration["Email:ApiKey"]
                    ?? throw new InvalidOperationException(
                        "Resend API key is not configured.");
            });

            // Cloudinary
            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;

                var account = new Account(
                    settings.CloudName,
                    settings.ApiKey,
                    settings.ApiSecret);

                return new Cloudinary(account);
            });


            services.AddHttpClient<IResend, ResendClient>();

            // Background services
            services.AddHostedService<PasswordResetTokenCleanupService>();


            // services
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
            services.AddTransient<IImageStorage, ImageStorageService>();
            return services;
        }
    }
}
