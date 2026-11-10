using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ResultWithValue<IReadOnlyCollection<UserResponseDto>>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetAllUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<ResultWithValue<IReadOnlyCollection<UserResponseDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userReadRepository.GetAllActiveUsersAsync(
                request.Page, 
                request.PageSize, cancellationToken);

            return ResultWithValue<IReadOnlyCollection<UserResponseDto>>.Success(users);
        }
    }
}
