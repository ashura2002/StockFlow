using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();


        private Order(Guid userId)
        {
            UserId = userId;
        }

        public static Order Create(Guid userId)
        {
            return new Order(userId);
        }
    }
}
