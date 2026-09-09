using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Profiles.Queries
{
    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserWithProfileResponseDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;

        public GetProfileQueryHandler(
            ICurrentUserService currentUserService,
           IUserWriteRepository userWriteRepository)
        {
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
        }


        public async Task<UserWithProfileResponseDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");

            return new UserWithProfileResponseDto(
                currentUserId, 
                user.Email.Value, 
                user.Profile?.FirstName.Value, 
                user.Profile?.LastName.Value,
                user.Profile?.DateOfBirth, 
                user.Profile?.Address.Value, 
                user.Profile?.ProfilePictureUrl, 
                user.Profile?.ProfilePicturePublicId);
        }
    }
}
