using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed record UpdatePasswordCommand(string Password) : IRequest;
}
