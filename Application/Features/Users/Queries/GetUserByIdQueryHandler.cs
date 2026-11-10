using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResultWithValue<UserResponseDto>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICacheService _cacheService;

        public GetUserByIdQueryHandler(
            IUserReadRepository userReadRepository,
            ICacheService cacheService)
        {
            _userReadRepository = userReadRepository;
            _cacheService = cacheService;
        }

        public async Task<ResultWithValue<UserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"user:{request.UserId}";

            var cachedUser = _cacheService.GetCachedResource<UserResponseDto>(cacheKey);

            if (cachedUser is not null)
            {
                return ResultWithValue<UserResponseDto>.Success(cachedUser);
            }

            var user = await _userReadRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResultWithValue<UserResponseDto>.Failure(UserErrors.UserNotFound);

            var userResponse = new UserResponseDto(user.UserId, user.Email, user.Role, user.CreatedAt);

            _cacheService.Set(cacheKey, userResponse, TimeSpan.FromMinutes(5));

            return ResultWithValue<UserResponseDto>.Success(userResponse);
        }
    }
}
