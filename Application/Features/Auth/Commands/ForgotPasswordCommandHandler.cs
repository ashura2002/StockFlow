using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordResetTokenRepository _resetTokenRepository;
        private readonly IPasswordTokenGeneratorService _tokenGenerator;
        private readonly IEmailSenderService _emailSender;
        private readonly ITokenHasherService _tokenHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFrontendUrlService _frontendUrlService;

        public ForgotPasswordCommandHandler(
            IUserWriteRepository userWriteRepository,
            IPasswordResetTokenRepository resetTokenRepository,
            IPasswordTokenGeneratorService passwordTokenGeneratorService,
            IEmailSenderService emailSenderService,
            ITokenHasherService passwordResetTokenHasherService,
            IUnitOfWork unitOfWork,
            IFrontendUrlService frontendUrlService)
        {
            _userWriteRepository = userWriteRepository;
            _resetTokenRepository = resetTokenRepository;
            _tokenGenerator = passwordTokenGeneratorService;
            _emailSender = emailSenderService;
            _tokenHasher = passwordResetTokenHasherService;
            _unitOfWork = unitOfWork;
            _frontendUrlService = frontendUrlService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userWriteRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null) return;

            // Generate random token
            var rawToken = _tokenGenerator.Generate();

            // Hash token using SHA-256
            var tokenHash = _tokenHasher.Hash(rawToken);

            // Token is valid for 15 minutes
            var expiresAt = DateTime.UtcNow.AddMinutes(15);

            // Create domain entity
            var resetToken = PasswordResetToken.Create(user.Id, tokenHash, expiresAt);
            _resetTokenRepository.Add(resetToken);

            // Persist reset token
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resetUrl = _frontendUrlService.CreatePasswordResetUrl(rawToken);

            // Send raw token to the user's email
            await _emailSender.SendAsync(
                user.Email.Value, 
                "Reset your password",
                 $"Click this link to reset your password: {resetUrl}",
                cancellationToken);
        }
    }
}
