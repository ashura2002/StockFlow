namespace Application.Dtos
{
    public sealed record CategoryWithProductsResponseDto(
        Guid CategoryId,
        string CategoryName,
        string? CategoryDescriptions,
        IReadOnlyCollection<ProductResponseDto> Products);
}
