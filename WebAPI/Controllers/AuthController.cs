using Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.RequestDtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public AuthController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediatR.Send(command, cancellationToken);
            return new LoginResponse("Login successfully.", result);
        }
    }
}
