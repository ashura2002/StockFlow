
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed record CancelMyOrderCommand(Guid OrderId) : IRequest;
}
