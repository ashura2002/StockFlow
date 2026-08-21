using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Profiles.Queries
{
    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserWithProfileResponseDto>
    {
        private readonly IProfileReadRepository _profileReadRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetProfileQueryHandler(
            IProfileReadRepository profileReadRepository,
            ICurrentUserService currentUserService)
        {
            _profileReadRepository = profileReadRepository;
            _currentUserService = currentUserService;
        }


        public async Task<UserWithProfileResponseDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var profile = await _profileReadRepository.GetProfileAsync(currentUserId, cancellationToken)??
                throw new DomainNotFoundException("Profile not found");

            return profile;
        }
    }
}
