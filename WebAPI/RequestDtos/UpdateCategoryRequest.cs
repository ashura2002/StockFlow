
namespace WebAPI.RequestDtos
{
    public sealed record UpdateCategoryRequest
    {
        public required string CategoryName { get; set; }

        public string? Description { get; set; }

    }
}
