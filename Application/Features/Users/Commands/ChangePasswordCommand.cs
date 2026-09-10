
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed record ChangePasswordCommand(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword) : IRequest;
}
