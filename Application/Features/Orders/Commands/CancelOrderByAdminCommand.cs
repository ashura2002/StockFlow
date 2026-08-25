using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed record CancelOrderByAdminCommand(Guid OrderId) : IRequest;
}
