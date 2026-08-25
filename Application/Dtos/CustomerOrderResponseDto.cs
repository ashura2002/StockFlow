using Domain.Enums;

namespace Application.Dtos
{
    public sealed record CustomerOrderResponseDto(
        Guid OrderId,
        IReadOnlyCollection<OrderItemResponse> Items,
        decimal TotalPrice,
        OrderStatus Status,
        DateTime OrderedAt);
}
