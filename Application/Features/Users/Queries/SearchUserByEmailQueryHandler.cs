using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries
{
    public sealed class SearchUserByEmailQueryHandler : IRequestHandler<SearchUserByEmailQuery, IReadOnlyCollection<UserResponseDto>>
    {
        private readonly IUserReadRepository _userReadRepository;
        public SearchUserByEmailQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }
        public async Task<IReadOnlyCollection<UserResponseDto>> Handle(SearchUserByEmailQuery request, CancellationToken cancellationToken)
        {

            return await _userReadRepository.GetUserByEmailAsync(
                request.Email, 
                request.Page, 
                request.PageSize, 
                cancellationToken);
        }
    }
}
