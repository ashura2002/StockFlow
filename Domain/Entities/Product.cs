using Domain.Exceptions;
using Domain.ValueObjects;


namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public ProductNameVo ProductName { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public Guid SupplierId { get; private set; }
        public Supplier Supplier { get; private set; } = null!;
          
        public string? ProductDescriptions { get; private set; }
        public string? ProductImageUrl { get; private set; }
        public string? ProductImagePublicId { get; private set; }


        private Product(
            ProductNameVo productName,
            decimal price,
            int stock,
            Guid categoryId,
            Guid supplierId,
            string?productDescriptions = null,
            string?productImageUrl = null,
            string? productImagePublicId = null)
        {
            ProductName = productName;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            SupplierId = supplierId;
            ProductDescriptions = productDescriptions;
            ProductImageUrl = productImageUrl;
            ProductImagePublicId = productImagePublicId;
        }

        public static Product Create(
          ProductNameVo productName,
          decimal price,
          int stock,
          Guid categoryId,
          Guid supplierId,
          string? productDescriptions = null,
          string? productImageUrl = null,
          string? productImagePublicId = null)
        {
            if (price <= 0)
                throw new DomainRuleException(
                    "Price must be greater than 0.");

            if (stock < 0)
                throw new DomainRuleException(
                    "Stock cannot be negative.");

            return new Product(
                productName, 
                price, 
                stock, 
                categoryId, 
                supplierId, 
                productDescriptions, 
                productImageUrl, 
                productImagePublicId);
        }


        public void UpdateProductName(ProductNameVo newProductName)
        {
            if (ProductName == newProductName) return;
            ProductName = newProductName;
            Touch();
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (Price == newPrice) return;

            if (newPrice <= 0)
                throw new DomainRuleException("Price must be greater than 0.");

            Price = newPrice;
            Touch();
        }

        public void UpdateProductImage(string? productImage)
        {
            if (ProductImageUrl == productImage) return;

            ProductImageUrl = productImage;
            Touch();
        }
    }
}