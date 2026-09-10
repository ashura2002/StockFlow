namespace WebAPI.RequestDtos
{
    public sealed record CreateSupplierRequest(
        string SupplierName,
        string Email,
        string PhoneNumber,
        string Address);

}
