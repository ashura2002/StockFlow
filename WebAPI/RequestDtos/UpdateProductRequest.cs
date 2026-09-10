

namespace WebAPI.RequestDtos
{
    public sealed record UpdateProductRequest
    (
        string ProductName,
        decimal Price,
        int Stock,
        string? Descriptions
    );
}
