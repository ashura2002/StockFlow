using Application.Dtos;

namespace Application.Interfaces
{
    public interface IDashboardReadRepository
    {
        Task<DashboardResponseDto> GetDashboardAsync(CancellationToken ct);
    }
}
