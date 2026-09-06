namespace WebAPI.RequestDtos
{
    public sealed record CreateProductRequest
    {
        public required string ProductName { get; set; }

        public required decimal Price { get; set; }

        public required int Stock { get; set; }
        public required Guid CategoryId { get; set; }
        public required Guid SupplierId { get; set; }
        public string? ProductDescriptions { get; set; }
    }
}
