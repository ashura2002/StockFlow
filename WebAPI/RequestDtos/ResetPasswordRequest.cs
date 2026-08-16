using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record ResetPasswordRequest
    {
        [Required(ErrorMessage = "Raw token is required")]
        public required string RawToken { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public required string NewPassword { get; set; }
    }
}
