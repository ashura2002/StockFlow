using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record CreateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        public required string CategoryName { get; set; }

        public string? Description { get; set; }
    }
}
