
namespace WebAPI.RequestDtos
{
    public record UpdateSupplierRequest
    {
        public required string SupplierName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }


    }
}
