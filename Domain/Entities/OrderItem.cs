using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order{ get; private set; } = null!;
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = null!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => UnitPrice * Quantity;


        private OrderItem(
            Guid orderId,
            Guid productId,
            int quantity,
            decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public static OrderItem Create(
            Guid orderId,
            Guid productId,
            int quantity,
            decimal unitPrice)
        {
            if (quantity <= 0)
                throw new DomainBadRequestException("Quantity must above 0.");

            if (unitPrice <= 0)
                throw new DomainBadRequestException(
                    "Unit price must be greater than 0.");

            return new OrderItem(
                    orderId,
                    productId,
                    quantity,
                    unitPrice);
        }
            
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainBadRequestException(
                    "Quantity must be greater than 0.");

            if (Quantity == quantity)
                return;

            Quantity = quantity;
            Touch();
        }
    }
}
