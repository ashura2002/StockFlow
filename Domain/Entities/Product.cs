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
        public DateTime? DeletedAt { get; private set; }

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
            EnsureProductNotDeleted("Can't update product name if product is deleted.");
            if (ProductName == newProductName) return;
            ProductName = newProductName;
            Touch();
        }

        public void UpdatePrice(decimal newPrice)
        {
            EnsureProductNotDeleted("Can't update product price if product is deleted.");
            if (Price == newPrice) return;

            if (newPrice <= 0)
                throw new DomainRuleException("Price must be greater than 0.");

            Price = newPrice;
            Touch();
        }

        public void UpdateProductStock(int newStock)
        {
            EnsureProductNotDeleted("Can't update product stock if product is deleted.");
            if (Stock == newStock) return;
            if (newStock < 0)
                throw new DomainRuleException(
                    "Stock cannot be negative.");

            Stock = newStock;
            Touch();
        }        

        public void UpdateProductDescriptions(string? newDescriptions)
        {
            EnsureProductNotDeleted("Can't update product descriptions if product is deleted.");
            if (ProductDescriptions == newDescriptions) return;
            ProductDescriptions = newDescriptions;
            Touch();
        }

        public void UpdateProductImage(string? productImage)
        {
            EnsureProductNotDeleted("Can't update product image if product is deleted.");
            if (ProductImageUrl == productImage) return;

            ProductImageUrl = productImage;
            Touch();
        }

        public void SoftDelete()
        {
            if (DeletedAt.HasValue) return;
            DeletedAt = DateTime.UtcNow;
            Touch();
        }

        public void DecreaseStock(int quantity)
        {
            if (Stock < quantity)
                throw new DomainRuleException("Out of stock.");

            Stock -= quantity; 
            Touch();
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainRuleException(
                    "Quantity must be greater than 0.");

            Stock += quantity;
            Touch();
        }

        private void EnsureProductNotDeleted(string message)
        {
            if (DeletedAt.HasValue)
                throw new DomainRuleException(message);
        }
    }
}