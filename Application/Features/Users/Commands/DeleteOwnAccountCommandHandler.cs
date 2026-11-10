using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class DeleteOwnAccountCommandHandler : IRequestHandler<DeleteOwnAccountCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOwnAccountCommandHandler(
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteOwnAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdAsync(currentUserId, cancellationToken);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);


            if (!string.Equals(
                user.Email.Value,
                request.Email,
                StringComparison.OrdinalIgnoreCase)) // ignore uppercase and lowercase when comparing.
                return Result.Failure(UserErrors.EmailNotMatch);

            user.SoftDelete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
