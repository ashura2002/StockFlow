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
        private readonly IPasswordResetTokenHasherService _tokenHasher;
        private readonly IUnitOfWork _unitOfWork;

        public ForgotPasswordCommandHandler(
            IUserWriteRepository userWriteRepository,
            IPasswordResetTokenRepository resetTokenRepository,
            IPasswordTokenGeneratorService passwordTokenGeneratorService,
            IEmailSenderService emailSenderService,
            IPasswordResetTokenHasherService passwordResetTokenHasherService,
            IUnitOfWork unitOfWork)
        {
            _userWriteRepository = userWriteRepository;
            _resetTokenRepository = resetTokenRepository;
            _tokenGenerator = passwordTokenGeneratorService;
            _emailSender = emailSenderService;
            _tokenHasher = passwordResetTokenHasherService;
            _unitOfWork = unitOfWork;
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

            // Send raw token to the user's email
            await _emailSender.SendAsync(
                user.Email.Value, 
                "Reset your password", 
                $"Your password reset token is: {rawToken}",
                cancellationToken);
        }
    }
}
