using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record UpdateProductRequest
    {
        [Required(ErrorMessage ="Product name is required")]
        public required string ProductName { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "Product stock is required")]
        public required int Stock { get; set; }
        public string? Descriptions { get; set; }
    }
}
