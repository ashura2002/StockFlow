using Application.Dtos;

namespace Application.Interfaces
{
    public interface IOrderReadRepository
    {
        Task<AdminOrderResponseDto?> GetOrderByIdAsync(
            Guid orderId, 
            CancellationToken ct);

        Task<IReadOnlyCollection<AdminOrderResponseDto>> GetAllPendingOrdersAsync(
            int page, 
            int pageSize, 
            CancellationToken ct);

        Task<IReadOnlyCollection<AdminOrderResponseDto>> GetAllConfirmOrdersAsync(
           int page,
           int pageSize,
           CancellationToken ct);

        Task<IReadOnlyCollection<AdminOrderResponseDto>> GetAllCancelledOrdersAsync(
            int page,
            int pageSize,
            CancellationToken ct);

        Task<IReadOnlyCollection<AdminOrderResponseDto>> GetAllCompletedOrdersAsync(
            int page,
            int pageSize,
            CancellationToken ct);

        Task<IReadOnlyCollection<CustomerOrderResponseDto>> GetAllMyOrdersAsync(
            int page, 
            int pageSize, 
            Guid userId, 
            CancellationToken ct);

        Task<CustomerOrderResponseDto?> GetMyOrderByIdAsync(
            Guid orderId, 
            Guid userId, 
            CancellationToken ct);
    }
}
