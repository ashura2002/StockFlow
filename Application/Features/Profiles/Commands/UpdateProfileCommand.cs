using MediatR;

namespace Application.Features.Profiles.Commands
{
    public sealed record UpdateProfileCommand(
        string FirstName, 
        string LastName, 
        string Address) : IRequest;
}
