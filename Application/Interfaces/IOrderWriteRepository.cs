
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrderWriteRepository
    {
        void Add(Order order);
        void Remove(Order order);
        Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken ct);
    }
}
