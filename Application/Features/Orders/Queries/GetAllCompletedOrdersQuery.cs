using Application.Dtos;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public record GetAllCompletedOrdersQuery(int Page, int PageSize) : IRequest<IReadOnlyCollection<AdminOrderResponseDto>>;
}
