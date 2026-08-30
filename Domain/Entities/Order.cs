using Domain.Enums;
using Domain.Events;
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
        public decimal TotalPrice => _orderItems.Sum(item => item.UnitPrice * item.Quantity);


        private Order(Guid userId)
        {
            UserId = userId;
            Status = OrderStatus.Pending;
        }

        public static Order Create(Guid userId)
        {
            var order = new Order(userId);
            order.RaiseEvent(new OrderCreatedDomainEvent(order.Id, order.UserId));
            return order;
        }


        public void AddItem(
            Guid productId,
            int quantity,
            decimal unitPrice)
        {
            if (quantity <= 0)
                throw new DomainBadRequestException(
                    "Quantity must be greater than 0.");

            var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
                return;
            }
            var newItem = OrderItem.Create(productId, quantity, unitPrice);
            _orderItems.Add(newItem);
            Touch();
        }

        public void RemoveItem(Guid productId)
        {
            var item = _orderItems.FirstOrDefault(i => 
            i.ProductId == productId);
            if (item is null) 
                return;

            _orderItems.Remove(item);
            Touch();
        }


        public void ConfirmOrder()
        {
            if (Status == OrderStatus.Confirmed) 
                return;

            EnsureIsPending("Only pending order can be confirmed.");

            Status = OrderStatus.Confirmed;
            RaiseEvent(new OrderConfirmedDomainEvent(Id, UserId));
            Touch();
        }

        public bool CancelOrder(OrderCancellationSource source)
        {
            if (Status == OrderStatus.Cancelled) 
                return false;

            EnsureIsPending("Only pending order can be cancelled.");

            Status = OrderStatus.Cancelled;
            // include the cancellation source so the domain event handler
            // can determine whether to notify the admin or the customer.
            RaiseEvent(new OrderCancelledDomainEvent(Id, UserId, source));
            Touch();
            return true;
        }

        public void CompleteOrder()
        {
            if (Status == OrderStatus.Completed) return;

            if (Status != OrderStatus.Confirmed)
                throw new DomainBadRequestException("Only confirmed orders can be completed");

            Status = OrderStatus.Completed;
            RaiseEvent(new OrderCompletedDomainEvent(Id, UserId));
            Touch();
        }

        public void EnsureIsPending(string message)
        {
            if (Status != OrderStatus.Pending)
                throw new DomainBadRequestException(message);
        }
    }
}

