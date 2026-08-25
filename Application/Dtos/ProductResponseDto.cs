
namespace Application.Dtos
{
    public sealed record ProductResponseDto(
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Stock,
        string Category,
        string Supplier,
        string? ProductDescriptions,
        string? ProductImageUrl,
        string? ProductImagePublicId);
}
