
using Domain.Enums;

namespace Application.Common.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error CategoryNotFound = new("Category.NotFound", "Category not found.", ErrorStatus.NotFound);
        public static readonly Error CategoryNameExisted= new("Category.CategoryNameExisted", "Category name already existed.", ErrorStatus.Conflict);
    }
}
