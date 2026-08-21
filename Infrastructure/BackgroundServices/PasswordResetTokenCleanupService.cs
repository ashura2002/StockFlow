using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public sealed class PasswordResetTokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PasswordResetTokenCleanupService> _logger;

        public PasswordResetTokenCleanupService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<PasswordResetTokenCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Starting password reset token cleanup...");

                // Create a new DI scope because the repository is registered as Scoped.
                using var scope = _serviceScopeFactory.CreateScope();

                // Resolve the scoped repository from the newly created scope.
                var repository = scope.ServiceProvider.GetRequiredService<IPasswordResetTokenRepository>();

                // Cleanup expired and used password reset tokens.
                await repository.DeleteExpiredAndUsedTokenAsync(stoppingToken);

                _logger.LogInformation("Password reset token cleanup completed.");

                // Run the cleanup once every 24 hours.
                // The cancellation token allows the delay to stop when the application shuts down.
                 await Task.Delay(
                    TimeSpan.FromHours(24),
                    stoppingToken);
            }
        }
    }
}
