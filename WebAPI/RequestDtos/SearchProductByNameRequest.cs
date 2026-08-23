using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public class SearchProductByNameRequest
    {
        [Required(ErrorMessage = "Product name is required")]
        public required string ProductName { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
