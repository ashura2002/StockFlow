using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserWriteRepository _userWriteRepository;

        public GetUserByIdQueryHandler(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userWriteRepository.GetUserByIdAsync(request.UserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");

            return new UserDto(
                user.Id, 
                user.Email.Value, 
                user.Role,
                user.CreatedAt);
        }
    }
}
