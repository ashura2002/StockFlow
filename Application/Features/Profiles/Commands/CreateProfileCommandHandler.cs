using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Profiles.Commands
{
    public sealed class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Guid>
    {
        private readonly IProfileWriteRepository _profile;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProfileCommandHandler(
            IProfileWriteRepository profile,
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork
            )
        {
            _profile = profile;
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var firstnameVo = FirstNameVo.Create(request.FirstName);
            var lastNameVo = LastNameVo.Create(request.LastName);
            var addressVo = AddressVo.Create(request.Address);

            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken)??
                throw new DomainNotFoundException("User not found.");

            Console.WriteLine(user);


            user.CreateProfile(firstnameVo,lastNameVo, request.DateOfBirth, addressVo);
            _profile.Add(user.Profile!);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Profile!.Id;
        }
    }
}
