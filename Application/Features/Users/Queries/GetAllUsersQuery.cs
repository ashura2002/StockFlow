using Application.Common.Results;
using Application.Dtos;
using MediatR;


namespace Application.Features.Users.Queries
{
    public record GetAllUsersQuery(int Page, int PageSize) : IRequest<ResultWithValue<IReadOnlyCollection<UserResponseDto>>>;
}
