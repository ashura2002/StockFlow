
namespace WebAPI.RequestDtos
{
    public record UpdateSupplierRequest
    (
    string SupplierName,
    string Email,
    string PhoneNumber,
    string Address);
}
