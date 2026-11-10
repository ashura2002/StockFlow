using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
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

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var user = await _userWriteRepository.GetUserByIdAsync(currentUserId, cancellationToken);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            if (!_passwordService.Verify(request.CurrentPassword, user.Password.Value))
                return Result.Failure(AuthErrors.IncorrectPassword);


            var newHashPassword = _passwordService.HashPassword(request.NewPassword);
            user.UpdatePassword(PasswordVo.Create(newHashPassword));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
