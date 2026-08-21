using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public record UpdateSupplierRequest
    {
        [Required(ErrorMessage = "Supplier mame is required")]
        public required string SupplierName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "Phone number is required")]
        public required string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Address is required")]
        public required string Address { get; set; }


    }
}
