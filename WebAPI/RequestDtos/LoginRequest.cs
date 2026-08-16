using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5)]
        public required string Password { get; set; }
    }
}
