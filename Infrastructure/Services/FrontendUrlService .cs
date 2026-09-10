using Application.Interfaces;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public sealed class FrontendUrlService : IFrontendUrlService
    {
        private readonly FrontendSettings _settings;

        public FrontendUrlService(IOptions<FrontendSettings> options)
        {
            _settings = options.Value;
        }
        public string CreatePasswordResetUrl(string rawToken)
        {
            return $"{_settings.BaseUrl.TrimEnd('/')}/reset-password?token={Uri.EscapeDataString(rawToken)}";
        }
    }
}
