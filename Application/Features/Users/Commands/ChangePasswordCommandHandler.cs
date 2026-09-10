using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public ChangePasswordCommandHandler(
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var user = await _userWriteRepository.GetUserByIdAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");

            if (!_passwordService.Verify(request.CurrentPassword, user.Password.Value))
                throw new DomainRuleViolationException("Incorrect password.");

            var newHashPassword = _passwordService.HashPassword(request.NewPassword);
            user.UpdatePassword(PasswordVo.Create(newHashPassword));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
