using Domain.Exceptions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

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



        private Product(
            ProductNameVo productName,
            decimal price,
            int stock,
            Guid categoryId,
            Guid supplierId)
        {
            ProductName = productName;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            SupplierId = supplierId;
        }

        public static Product Create(
          ProductNameVo productName,
          decimal price,
          int stock,
          Guid categoryId,
          Guid supplierId)
        {
            if (price <= 0)
                throw new DomainBadRequestException(
                    "Price must be greater than 0.");

            if (stock < 0)
                throw new DomainBadRequestException(
                    "Stock cannot be negative.");

            return new Product(productName, price, stock, categoryId, supplierId);
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
                throw new DomainBadRequestException("Price must be greater than 0.");

            Price = newPrice;
            Touch();
        }
    }
}
