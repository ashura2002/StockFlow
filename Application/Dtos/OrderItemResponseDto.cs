namespace Application.Dtos
{
    public sealed record OrderItemResponseDto(
        Guid OrderItemId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice);
}
