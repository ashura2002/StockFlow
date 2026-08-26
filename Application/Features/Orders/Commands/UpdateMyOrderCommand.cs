using Application.Features.OrderItems.Commands;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed record UpdateMyOrderCommand(Guid OrderId, IReadOnlyCollection<CreateOrderItemCommand> Items) : IRequest;
}
