using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Dashboards.Queries
{
    public sealed record GetDashboardQuery : IRequest<ResultWithValue<DashboardResponseDto>>;
}
