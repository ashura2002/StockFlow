using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IPasswordResetTokenHasherService _passwordResetTokenHasherService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(
            IUserWriteRepository userWriteRepository,
            IPasswordService passwordService,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            IPasswordResetTokenHasherService passwordResetTokenHasherService,
            IUnitOfWork unitOfWork)
        {
            _userWriteRepository = userWriteRepository;
            _passwordService = passwordService;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _passwordResetTokenHasherService = passwordResetTokenHasherService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // hash with SHA-256
            var tokenHash = _passwordResetTokenHasherService.Hash(request.RawToken);

            var resetToken = await _passwordResetTokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

            // Return without revealing whether the reset token exists.
            if (resetToken is null)
                throw new DomainRuleException("Invalid or expired password reset token.");

            if (resetToken.IsUsed || resetToken.IsExpired)
                throw new DomainRuleException("Invalid or expired password reset token.");

            var user = await _userWriteRepository.GetUserByIdAsync(resetToken.UserId, cancellationToken);
            if (user is null) return;

            var newPassword = PasswordVo.Create(request.NewPassword);

            // hash with bcrypt
            var hashPassword = _passwordService.HashPassword(newPassword.Value);
            user.UpdatePassword(PasswordVo.Create(hashPassword));

            resetToken.MarkAsUsed();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
