using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public class SearchUserByEmailRequest
    {
        [Required(ErrorMessage = "Email is required of searching user.")]
        public required string Email { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
