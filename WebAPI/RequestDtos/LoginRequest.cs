using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public required string Password { get; set; }
    }
}
