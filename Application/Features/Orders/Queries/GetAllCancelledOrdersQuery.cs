using Application.Dtos;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed record GetAllCancelledOrdersQuery(int Page, int PageSize) : IRequest<IReadOnlyCollection<AdminOrderResponseDto>>;
}
