using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Order : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public OrderStatus Status { get; private set; }
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // total price of an Order 
        // computed in memory for domain logic.
        public decimal TotalPrice => _orderItems.Sum(item => item.UnitPrice * item.Quantity);


        private Order(Guid userId)
        {
            UserId = userId;
            Status = OrderStatus.Pending;
        }

        public static Order Create(Guid userId)
        {
            var order = new Order(userId);
            // raise event here
            return order;
        }


        public void AddItem(
            Guid productId,
            int quantity,
            decimal unitPrice)
        {
            if (quantity <= 0)
                throw new DomainRuleException(
                    "Quantity must be greater than 0.");

            var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
                return;
            }
            var newItem = OrderItem.Create(productId, quantity, unitPrice);
            _orderItems.Add(newItem);
        }

        public void RemoveItem(Guid productId)
        {
            var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
            if (item is null) return;

            _orderItems.Remove(item);
            Touch();
        }


        public void ConfirmOrder()
        {
            if (Status == OrderStatus.Confirmed) return;

            if (Status != OrderStatus.Pending)
                throw new DomainRuleException("Only pending order can be confirmed.");

            Status = OrderStatus.Confirmed;
            // raise event here
            Touch();
        }

        public bool CancelOrder()
        {
            if (Status == OrderStatus.Cancelled) return false;

            if(Status != OrderStatus.Pending)
                throw new DomainRuleException("Only pending order can be cancelled.");

            Status = OrderStatus.Cancelled;
            Touch();
            return true;
        }

        public void CompleteOrder()
        {
            if (Status == OrderStatus.Completed) return;

            if (Status != OrderStatus.Confirmed)
                throw new DomainRuleException("Only confirmed orders can be completed");

            Status = OrderStatus.Completed;
            Touch();
        }

        public void EnsureCanBeModified()
        {
            if (Status != OrderStatus.Pending)
                throw new DomainRuleException("Only pending orders can be modify.");
        }
    }
}
