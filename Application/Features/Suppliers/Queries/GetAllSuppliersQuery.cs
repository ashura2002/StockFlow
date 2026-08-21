using Application.Dtos;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public sealed record GetAllSuppliersQuery(
        int Page, 
        int PageSize):IRequest<IReadOnlyCollection<SupplierResponseDto>>
    {
    }
}
