using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userReadRepository.GetUserByIdAsync(request.UserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");
        }
    }
}
