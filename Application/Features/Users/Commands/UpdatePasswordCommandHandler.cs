using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPasswordService _passwordService;

        public UpdatePasswordCommandHandler(
            IUserWriteRepository userReadRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IPasswordService passwordService)
        {
            _userWriteRepository = userReadRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _passwordService = passwordService;
        }

        public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var passwordVo = PasswordVo.Create(request.Password);

            var user = await _userWriteRepository.GetUserByIdAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");
            if (user.Id != currentUserId)
                throw new DomainUnauthorizedException("Not allowed to modify other user's password.");
            var hashPassword = _passwordService.HashPassword(passwordVo.Value);

            user.UpdatePassword(PasswordVo.Create(hashPassword));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

