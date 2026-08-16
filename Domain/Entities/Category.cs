
namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; private set; }

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        public string? Description { get; private set; }


        private Category(string categoryName, string? description = null)
        {
            CategoryName = categoryName;
            Description = description;
        }

        public static Category Create(string categoryName, string? description = null)
        {
            return new Category(categoryName, description);
        }
    }
}
