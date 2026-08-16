using Application.Interfaces;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Resend;

namespace Infrastructure.Services
{
    public sealed class EmailSenderService : IEmailSenderService
    {
        private readonly IResend _resend;
        private readonly EmailSettings _emailSettings;

        public EmailSenderService(IResend resend, IOptions<EmailSettings> options)
        {
            _resend = resend;
            _emailSettings = options.Value;
        }
        public async Task SendAsync(string recipient, string subject, string body, CancellationToken cancellationToken)
        {
            var message = new EmailMessage
            {
                From = _emailSettings.From,
                To = recipient,
                Subject = subject,
                HtmlBody = body
            };

            await _resend.EmailSendAsync(message, cancellationToken);
        }
    }
}
