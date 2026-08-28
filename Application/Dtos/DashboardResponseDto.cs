namespace Application.Dtos
{
    public sealed record DashboardResponseDto(
        decimal TotalRevenue,
        int TotalOrders,
        int CompletedOrders,
        int PendingOrders,
        int CancelledOrders,
        int ConfirmedOrders);
}
