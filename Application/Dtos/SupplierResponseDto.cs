namespace Application.Dtos
{
    public sealed record SupplierResponseDto(
        Guid SupplierId,
        string SupplierName,
        string Email,
        string PhoneNumber,
        string Address);
}
