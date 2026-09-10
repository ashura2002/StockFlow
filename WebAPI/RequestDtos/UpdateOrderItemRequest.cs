using Application.Features.OrderItems.Commands;

namespace WebAPI.RequestDtos
{
    public sealed record UpdateOrderItemRequest
    (
        IReadOnlyCollection<CreateOrderItemCommand> OrderItems
    );
}
