using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetAllInActiveUsersQueryHandler : IRequestHandler<GetAllInActiveUsersQuery, IReadOnlyCollection<UserDto>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetAllInActiveUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<IReadOnlyCollection<UserDto>> Handle(GetAllInActiveUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userReadRepository.GetAllInActiveUsersAsync(cancellationToken);
            return users;
        }
    }
}
