
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed record ConfirmOrderCommand(Guid OrderId) : IRequest;
}
