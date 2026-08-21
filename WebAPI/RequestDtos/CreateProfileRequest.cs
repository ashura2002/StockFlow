using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record CreateProfileRequest
    {
        [Required(ErrorMessage = "Firstname is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public required DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public required string Address { get; set; }
    }
}
