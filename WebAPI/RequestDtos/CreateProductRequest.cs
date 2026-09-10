namespace WebAPI.RequestDtos
{
    public sealed record CreateProductRequest(
        string ProductName,
        decimal Price,
        int Stock,
        Guid CategoryId,
        Guid SupplierId,
        string? ProductDescriptions);
}
