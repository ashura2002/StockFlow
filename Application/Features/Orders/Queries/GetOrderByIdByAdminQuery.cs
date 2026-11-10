using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed record GetOrderByIdByAdminQuery(Guid OrderId) : IRequest<ResultWithValue<AdminOrderResponseDto>>;
}
