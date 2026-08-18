using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyCollection<UserResponseDto>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetAllUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<IReadOnlyCollection<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userReadRepository.GetAllActiveUsersAsync(
                request.Page, 
                request.PageSize, cancellationToken);
            return users;
        }
    }
}
