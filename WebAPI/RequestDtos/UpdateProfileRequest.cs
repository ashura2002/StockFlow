using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record UpdateProfileRequest
    {
        [Required(ErrorMessage = "Firstname is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public required string Address { get; set; }

    }
}
