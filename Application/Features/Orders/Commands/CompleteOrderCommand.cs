using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed record CompleteOrderCommand(Guid OrderId) : IRequest;
}
