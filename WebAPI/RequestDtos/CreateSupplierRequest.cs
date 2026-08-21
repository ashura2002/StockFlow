using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public class CreateSupplierRequest
    {
        [Required(ErrorMessage ="Name of supplier is required")]
        public required string SupplierName { get; set; }

        [Required(ErrorMessage = "Email of supplier is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone number of supplier is required")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address of supplier is required")]
        public required string Address { get; set; }
    }
}
