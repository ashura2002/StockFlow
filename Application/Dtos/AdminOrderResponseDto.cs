using Domain.Enums;

namespace Application.Dtos
{
    public sealed record AdminOrderResponseDto(
        Guid OrderId,
        string Email,
        IReadOnlyCollection<OrderItemResponseDto> Items,
        decimal TotalPrice,
        OrderStatus Status,
        DateTime OrderedAt);
}
