using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public CategoryNameVo CategoryName { get; private set; }

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        public string? Description { get; private set; }


        private Category(CategoryNameVo categoryName, string? description = null)
        {
            CategoryName = categoryName;
            Description = description;
        }

        public static Category Create(CategoryNameVo categoryName, string? description = null)
        {
            return new Category(categoryName, description);
        }

        public void UpdateCategoryName(CategoryNameVo newCategoryName)
        {
            if (CategoryName == newCategoryName) return;

            CategoryName = newCategoryName;
            Touch();
        }

        public void UpdateDescription(string? newDescription)
        {
            if (Description == newDescription) return;

            Description = newDescription;
            Touch();
        }
    }
}
