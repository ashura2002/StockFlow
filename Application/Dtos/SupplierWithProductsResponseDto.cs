namespace Application.Dtos
{
    public sealed record SupplierWithProductsResponseDto(
        Guid SupplierId,
        string SupplierName,
        string Email,
        string Phonenumber,
        string Address,
        IReadOnlyCollection<ProductResponseDto> Products);
}
