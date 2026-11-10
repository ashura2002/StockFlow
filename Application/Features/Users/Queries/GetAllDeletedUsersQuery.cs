using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Users.Queries
{
    public record GetAllDeletedUsersQuery() : IRequest<ResultWithValue<IReadOnlyCollection<DeletedUserResponseDto>>>;
}
