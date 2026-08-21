
using MediatR;

namespace Application.Features.Profiles.Commands
{
    public sealed record CreateProfileCommand(
        string FirstName, 
        string LastName, 
        DateOnly DateOfBirth, 
        string Address) : IRequest<Guid>;
}
