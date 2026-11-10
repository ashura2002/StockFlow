using Application.Common.Results;
using Application.Dtos;
using MediatR;


namespace Application.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<ResultWithValue<UserResponseDto>>;
}
