using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
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

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"user:{request.UserId}";

            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _userReadRepository.GetUserByIdAsync(request.UserId, cancellationToken),
                TimeSpan.FromMinutes(5)) ??
                throw new DomainNotFoundException("User not found.");
        }
    }
}
