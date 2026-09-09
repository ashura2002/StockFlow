using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;
using System.Security.Cryptography;

namespace Application.Features.Users.Commands
{
    public sealed class RequestAccountRestoreCommandHandler : IRequestHandler<RequestAccountRestoreCommand>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ITokenHasherService _tokenHasherService;

        public RequestAccountRestoreCommandHandler(
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork,
            IEmailSenderService emailSenderService,
            ITokenHasherService tokenHasherService)
        {
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _emailSenderService = emailSenderService;
            _tokenHasherService = tokenHasherService;
        }

        public async Task Handle(RequestAccountRestoreCommand request, CancellationToken cancellationToken)
        {
            var email = EmailVo.Create(request.Email);

            var user = await _userWriteRepository.GetDeletedUserByEmailAsync(email.Value, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");

            var code = RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();

           var hashCode =  _tokenHasherService.Hash(code);
            var expiresAt = DateTime.UtcNow.AddMinutes(5);

            user.SetRestoreCode(hashCode, expiresAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _emailSenderService.SendAsync(
                 user.Email.Value,
                 "Account Restoration Verification Code",
                 $"Your account restoration verification code is: {code}\n\n" +
                 "This code will expire in 5 minutes. " +
                 "If you did not request to restore your account, please ignore this email.",
                 cancellationToken);
        }
    }
}
