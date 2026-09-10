
namespace WebAPI.RequestDtos
{
    public sealed record UpdateCategoryRequest(
        string CategoryName,
        string? Description);
}
