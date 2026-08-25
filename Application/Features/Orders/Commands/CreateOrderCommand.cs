
using Application.Features.OrderItems.Commands;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed record CreateOrderCommand(IReadOnlyCollection<CreateOrderItemCommand> Items) : IRequest<Guid>;
}
