using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed record RequestAccountRestoreCommand(string Email) : IRequest;
}
