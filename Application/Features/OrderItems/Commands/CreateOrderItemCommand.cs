

namespace Application.Features.OrderItems.Commands
{
    public sealed record CreateOrderItemCommand(Guid ProductId, int Quantity);
}
