using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Profiles.Commands
{
    public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProfileCommandHandler(
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var firstNameVo = FirstNameVo.Create(request.FirstName);
            var lastNameVo = LastNameVo.Create(request.LastName);
            var addressVo = AddressVo.Create(request.Address);

            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found");

            user.UpdateProfile(firstNameVo,lastNameVo,addressVo);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
