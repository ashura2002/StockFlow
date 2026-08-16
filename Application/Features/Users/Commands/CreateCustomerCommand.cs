using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed record CreateCustomerCommand(
        string Email,  
        string Password):IRequest<Guid>;
}
