
namespace WebAPI.RequestDtos
{
    public sealed record CreateCategoryRequest
    {
        public required string CategoryName { get; set; }

        public string? Description { get; set; }
    }
}
