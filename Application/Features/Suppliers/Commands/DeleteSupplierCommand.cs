using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed record DeleteSupplierCommand(Guid SupplierId) : IRequest;
}
