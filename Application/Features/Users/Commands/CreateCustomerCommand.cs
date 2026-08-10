using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Commands
{
    public record CreateCustomerCommand(
        string Email,  
        string Password):IRequest<Guid>;
}
