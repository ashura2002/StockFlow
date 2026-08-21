using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record CreateProductRequest
    {
        [Required(ErrorMessage = "Product name is required")]
        public required string ProductName { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "Product stock is required")]
        public required int Stock { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        public required Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Product supplier is required")]
        public required Guid SupplierId { get; set; }
        public string? ProductDescriptions { get; set; }
    }
}
