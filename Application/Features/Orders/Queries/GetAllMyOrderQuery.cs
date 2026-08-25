using Application.Dtos;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed record GetAllMyOrderQuery(int Page, int PageSize) : IRequest<IReadOnlyCollection<CustomerOrderResponseDto>>;
}
