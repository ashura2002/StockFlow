using Domain.Exceptions;


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
            Guid productId,
            int quantity,
            decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        internal static OrderItem Create(
            Guid productId,
            int quantity,
            decimal unitPrice)
        {
            if (productId == Guid.Empty)
                throw new DomainBadRequestException("Atleast one product ID is required.");

            if (quantity <= 0)
                throw new DomainBadRequestException("Quantity must above 0.");

            if (unitPrice <= 0)
                throw new DomainBadRequestException(
                    "Unit price must be greater than 0.");

            return new OrderItem(
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
