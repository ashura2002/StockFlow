using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed record UpdateSupplierCommand(
        Guid SupplierId,
        string SupplierName,
        string Email,
        string PhoneNumber,
        string Address) : IRequest;
}
