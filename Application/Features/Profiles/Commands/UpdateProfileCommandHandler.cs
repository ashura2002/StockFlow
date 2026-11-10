using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Profiles.Commands
{
    public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
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

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var firstNameVo = FirstNameVo.Create(request.FirstName);
            var lastNameVo = LastNameVo.Create(request.LastName);
            var addressVo = AddressVo.Create(request.Address);

            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken);
            if(user is null)
                return Result.Failure(UserErrors.UserNotFound);

            user.UpdateProfile(firstNameVo, lastNameVo, addressVo);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
