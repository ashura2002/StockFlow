using Application.Dtos;
using Application.Features.Dashboards.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesConstant.Admin)]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardResponseDto>> GetDashboardInformation(CancellationToken cancellationToken)
        {
            var query = new GetDashboardQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
