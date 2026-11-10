using Application.Dtos;
using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebAPI.Constants;
using WebAPI.Extensions;
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
            var command = new CreateCustomerCommand(
                request.Email,
                request.Password,
                request.ConfirmPassword);

            var result = await _mediatR.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpPut("me/password")]
        public async Task<ActionResult> UpdatePassword(
            [FromBody] ChangePasswordRequest request,
            CancellationToken cancellationToken)
        {
            var command = new ChangePasswordCommand(
                request.CurrentPassword,
                request.NewPassword,
                request.ConfirmNewPassword);

           var result = await _mediatR.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }


        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet("{userId:guid}")]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(userId);

            var result = await _mediatR.Send(query, cancellationToken);

            return this.ToActionResult(result);
        }

        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet("active")]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<UserResponseDto>>> GetAllUsers(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var queries = new GetAllUsersQuery(request.Page, request.PageSize);
            var result = await _mediatR.Send(queries, cancellationToken);

            return this.ToActionResult(result);
        }

        [Authorize(Roles = RolesConstant.Admin)]
        [HttpGet("deleted")]
        public async Task<ActionResult<IReadOnlyCollection<DeletedUserResponseDto>>> GetAllDeletedUsers(CancellationToken cancellationToken)
        {
            var queries = new GetAllDeletedUsersQuery();
            var result = await _mediatR.Send(queries, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpDelete("me")]
        public async Task<ActionResult> DeleteOwnAccount(
            [FromBody] DeleteOwnAccountRequest request,
            CancellationToken cancellationToken)
        {
            var command = new DeleteOwnAccountCommand(request.Email);
            var result = await _mediatR.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpGet("search")]
        [Authorize(Roles = RolesConstant.Admin)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<UserResponseDto>>> SearchUserByEmail(
            [FromQuery] SearchUserByEmailRequest request,
            CancellationToken cancellationToken)
        {
            var query = new SearchUserByEmailQuery(
                request.Email,
                request.Page,
                request.PageSize);
            var result = await _mediatR.Send(query, cancellationToken);

            return this.ToActionResult(result);
        }

    }
}

