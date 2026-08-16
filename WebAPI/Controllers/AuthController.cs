using Application.Features.Auth.Commands;
using MediatR;
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
        public async Task<ActionResult<LoginResponse>> Login(
            [FromBody] LoginRequest request, 
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediatR.Send(command, cancellationToken);
            return new LoginResponse("Login successfully.", result);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequest request, 
            CancellationToken cancellationToken)
        {
            var command = new ForgotPasswordCommand(request.Email);
            await _mediatR.Send(command, cancellationToken);
            return Ok(new
            {
                message = "If the email is registered, a password reset email has been sent."
            });
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(
            [FromBody] ResetPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var command = new ResetPasswordCommand(request.RawToken, request.NewPassword);
            await _mediatR.Send(command, cancellationToken);
            return Ok(new
            {
                message = "Password has been reset successfully."
            });
        }
        
    }
}
// background service for deleting hashtoken expired and used