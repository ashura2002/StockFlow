using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Dashboards.Queries
{
    public sealed class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResponseDto>
    {
        private readonly IDashboardReadRepository _dashboardReadRepository;
        public GetDashboardQueryHandler(IDashboardReadRepository dashboardReadRepository)
        {
            _dashboardReadRepository = dashboardReadRepository;
        }

        public async Task<DashboardResponseDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            return await _dashboardReadRepository.GetDashboardAsync(cancellationToken);
        }
    }
}
