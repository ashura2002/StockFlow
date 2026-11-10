using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries
{
    public sealed class SearchUserByEmailQueryHandler : IRequestHandler<SearchUserByEmailQuery, ResultWithValue<IReadOnlyCollection<UserResponseDto>>>
    {
        private readonly IUserReadRepository _userReadRepository;
        public SearchUserByEmailQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }
        public async Task<ResultWithValue<IReadOnlyCollection<UserResponseDto>>> Handle(SearchUserByEmailQuery request, CancellationToken cancellationToken)
        {

            var users = await _userReadRepository.GetUserByEmailAsync(
                request.Email, 
                request.Page, 
                request.PageSize, 
                cancellationToken);

            return ResultWithValue<IReadOnlyCollection<UserResponseDto>>.Success(users);
        }
    }
}
