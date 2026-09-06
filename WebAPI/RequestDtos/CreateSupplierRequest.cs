namespace WebAPI.RequestDtos
{
    public class CreateSupplierRequest
    {
        public required string SupplierName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }
}
