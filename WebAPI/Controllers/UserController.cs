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
    [Route("api/[controller]s")]
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
        public async Task<ActionResult<UserResponseDto>> GetUserById(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(userId);

            var result = await _mediatR.Send(query, cancellationToken);

            return new UserResponseDto(
                result.UserId,
                result.Email,
                result.Role,
                result.CreatedAt);
        }

        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet("active")]
        public async Task<ActionResult<IReadOnlyCollection<UserResponseDto>>> GetAllUsers(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var queries = new GetAllUsersQuery(request.Page, request.PageSize);
            var result = await _mediatR.Send(queries, cancellationToken);

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("password")]
        public async Task<ActionResult> UpdatePassword(
            [FromBody] UpdatePasswordRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePasswordCommand(request.Password);
            await _mediatR.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet("in-active")]
        public async Task<ActionResult<IReadOnlyCollection<UserResponseDto>>> GetAllInActiveUsers(CancellationToken cancellationToken)
        {
            var queries = new GetAllInActiveUsersQuery();
            var result = await _mediatR.Send(queries, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = RolesConstant.Customer)]
        [HttpDelete("me")]
        public async Task<ActionResult> DeleteOwnAccount(CancellationToken cancellationToken)
        {
            var command = new DeleteOwnAccountCommand();
            await _mediatR.Send(command, cancellationToken);
            return NoContent();
        }

    }
}

// add product image