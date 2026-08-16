using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class DeleteOwnAccountCommandHandler : IRequestHandler<DeleteOwnAccountCommand>
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

        public async Task Handle(DeleteOwnAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found");

            user.SoftDelete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
