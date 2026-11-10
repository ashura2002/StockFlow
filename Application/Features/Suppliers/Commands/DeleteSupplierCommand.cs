using Application.Common.Results;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed record DeleteSupplierCommand(Guid SupplierId) : IRequest<Result>;
}
