
using Application.Common.Results;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed record DeleteProductCommand(Guid ProductId) : IRequest<Result>;
}
