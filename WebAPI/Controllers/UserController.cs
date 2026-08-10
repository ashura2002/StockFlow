using Application.Dtos;
using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;
using WebAPI.RequestDtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public UserController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Guid>> Register(
            [FromBody] CustomerRegistrationRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateCustomerCommand(request.Email, request.Password);

            var result = await _mediatR.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetUserById),
                new { userId = result },
                result);
        }


        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserDto>> GetUserById(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(userId);

            var result = await _mediatR.Send(query, cancellationToken);

            return new UserDto(
                result.UserId, 
                result.Email, 
                result.Role,
                result.CreatedAt);
        }

        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UserDto>>> GetAllUsers(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var queries = new GetAllUsersQuery(request.Page, request.PageSize);
            var result = await _mediatR.Send(queries, cancellationToken);

            return Ok(result);
        }
    }
}
