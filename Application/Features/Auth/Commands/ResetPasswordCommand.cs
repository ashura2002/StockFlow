using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed  record ResetPasswordCommand(string RawToken, string NewPassword) : IRequest;
}
