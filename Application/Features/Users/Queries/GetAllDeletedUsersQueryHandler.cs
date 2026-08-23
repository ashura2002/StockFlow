using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetAllDeletedUsersQueryHandler : IRequestHandler<GetAllDeletedUsersQuery, IReadOnlyCollection<DeletedUserResponseDto>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetAllDeletedUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<IReadOnlyCollection<DeletedUserResponseDto>> Handle(GetAllDeletedUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userReadRepository.GetAllDeletedUsersAsync(cancellationToken);
            return users;
        }
    }
}
