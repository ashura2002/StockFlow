
using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class ConfirmAccountRestoreCommandHandler : IRequestHandler<ConfirmAccountRestoreCommand>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenHasherService _tokenHasherService;

        public ConfirmAccountRestoreCommandHandler(
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork,
            ITokenHasherService tokenHasherService)
        {
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _tokenHasherService = tokenHasherService;
        }

        public async Task Handle(ConfirmAccountRestoreCommand request, CancellationToken cancellationToken)
        {
            var email = EmailVo.Create(request.Email);

            var user = await _userWriteRepository.GetDeletedUserByEmailAsync(email.Value, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");

            var codeHash = _tokenHasherService.Hash(request.VerificationCode);

            user.VerifyRestoreCode(codeHash);
            user.RestoreAccount();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
