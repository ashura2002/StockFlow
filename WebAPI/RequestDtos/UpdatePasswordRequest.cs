using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record UpdatePasswordRequest
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5)]
        public required string Password { get; set; }
    }
}
