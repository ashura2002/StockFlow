namespace Application.Dtos
{
    public sealed record OrderItemResponse(
        Guid OrderItemId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice);
}
