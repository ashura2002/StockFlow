using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Profiles.Queries
{
    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ResultWithValue<UserWithProfileResponseDto>>
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


        public async Task<ResultWithValue<UserWithProfileResponseDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken);
            if (user is null)
                return ResultWithValue<UserWithProfileResponseDto>.Failure(UserErrors.UserNotFound);

            var result = new UserWithProfileResponseDto(
                user.Id,
                user.Email.Value,
                user.Profile?.FirstName.Value,
                user.Profile?.LastName.Value,
                user.Profile?.DateOfBirth,
                user.Profile?.Address.Value,
                user.Profile?.ProfilePictureUrl,
                user.Profile?.ProfilePicturePublicId);

            return ResultWithValue<UserWithProfileResponseDto>.Success(result);
        }
    }
}
