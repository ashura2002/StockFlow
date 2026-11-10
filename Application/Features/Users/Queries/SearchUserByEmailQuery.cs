using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Users.Queries
{
    public sealed record SearchUserByEmailQuery(
        string Email, 
        int Page, 
        int PageSize) : IRequest<ResultWithValue<IReadOnlyCollection<UserResponseDto>>>;
}
