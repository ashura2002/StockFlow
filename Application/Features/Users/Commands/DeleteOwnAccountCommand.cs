using MediatR;


namespace Application.Features.Users.Commands
{
    public sealed record DeleteOwnAccountCommand(string Email) : IRequest;
}
