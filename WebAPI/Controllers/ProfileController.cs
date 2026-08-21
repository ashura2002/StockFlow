using Application.Dtos;
using Application.Features.Profiles.Commands;
using Application.Features.Profiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.RequestDtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProfile(
            [FromBody] CreateProfileRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateProfileCommand(
                request.FirstName, 
                request.LastName, 
                request.DateOfBirth, 
                request.Address);

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProfileCommand(request.FirstName, request.LastName, request.Address);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("my-profile")]
        public async Task<ActionResult<UserWithProfileResponseDto>> GetProfile(CancellationToken cancellationToken)
        {
            var query = new GetProfileQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // Upload profile picture
        [HttpPatch("profile-picture")]
        public async Task<ActionResult<string>> UpdateProfilePicture(IFormFile file, CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();

            var result = await _mediator.Send(
                new UpdateProfilePictureCommand(
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length),
                cancellationToken);

            return Ok(result);
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteProfile(CancellationToken cancellationToken)
        {
            var command = new DeleteProfileCommand();
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}