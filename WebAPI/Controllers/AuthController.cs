using Application.Features.Auth.Commands;
using Application.Features.Auth.Responses;
using Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebAPI.Extensions;
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
        [EnableRateLimiting("LoginPolicy")]
        public async Task<ActionResult<TokenResponse>> Login(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediatR.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(
       [FromBody] ForgotPasswordRequest request,
       CancellationToken cancellationToken)
        {
            var command = new ForgotPasswordCommand(request.Email);

            var result = await _mediatR.Send(command, cancellationToken);

            if (result.IsFailue)
                return this.ToActionResult(result);

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
            var command = new ResetPasswordCommand(
                request.RawToken,
                request.NewPassword);

            var result = await _mediatR.Send(command, cancellationToken);

            if (result.IsFailue)
            {
                return this.ToActionResult(result);
            }

            return Ok(new
            {
                message = "Password has been reset successfully."
            });
        }


        [HttpPost("account-restore/request")]
        public async Task<ActionResult> RequestAccountRestore(
            [FromBody] AccountRestoreRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RequestAccountRestoreCommand(request.Email);

            var result = await _mediatR.Send(command, cancellationToken);

            return this.ToActionResult(result);
        }

        [HttpPost("account-restore/confirm")]
        public async Task<ActionResult> ConfirmAccountRestore(
         [FromBody] ConfirmAccountRestoreRequest request,
         CancellationToken cancellationToken)
        {
            var command = new ConfirmAccountRestoreCommand(
                request.Email,
                request.VerificationCode);

            var result = await _mediatR.Send(command, cancellationToken);

            return this.ToActionResult(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var result = await _mediatR.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
