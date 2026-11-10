using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Dashboards.Queries
{
    public sealed class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, ResultWithValue<DashboardResponseDto>>
    {
        private readonly IDashboardReadRepository _dashboardReadRepository;
        public GetDashboardQueryHandler(IDashboardReadRepository dashboardReadRepository)
        {
            _dashboardReadRepository = dashboardReadRepository;
        }

        public async Task<ResultWithValue<DashboardResponseDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var result =  await _dashboardReadRepository.GetDashboardAsync(cancellationToken);
            return ResultWithValue<DashboardResponseDto>.Success(result);
        }
    }
}
