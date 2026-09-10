using Application.Features.OrderItems.Commands;

namespace WebAPI.RequestDtos
{
    public sealed record CreateOrderRequest(IReadOnlyCollection<CreateOrderItemCommand> OrderItems);
}
