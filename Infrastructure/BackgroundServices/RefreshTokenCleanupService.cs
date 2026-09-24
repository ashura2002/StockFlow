using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public sealed class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<RefreshTokenCleanupService> _logger;

        public RefreshTokenCleanupService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<RefreshTokenCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Starting refresh token cleanup...");

                using var scope = _serviceScopeFactory.CreateScope();

                var repository = scope.ServiceProvider
                        .GetRequiredService<IRefreshTokenCleanupRepository>();

                await repository.DeleteExpiredAndRevokedTokensAsync(stoppingToken);

                _logger.LogInformation("Refresh token cleanup completed.");

                await Task.Delay(
                    TimeSpan.FromHours(24),
                    stoppingToken);
            }
        }
    }
}
