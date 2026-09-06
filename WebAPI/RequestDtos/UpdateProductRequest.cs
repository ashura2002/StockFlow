using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record UpdateProductRequest
    {
        public required string ProductName { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public string? Descriptions { get; set; }
    }
}
