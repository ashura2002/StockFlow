using Application.Common.Results;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed record CreateSupplierCommand(
        string SupplierName, 
        string Email, 
        string PhoneNumber, 
        string Address) : IRequest<ResultWithValue<Guid>>;
}
