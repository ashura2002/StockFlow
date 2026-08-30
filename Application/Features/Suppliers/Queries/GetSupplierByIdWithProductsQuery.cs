using Application.Dtos;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public sealed record GetSupplierByIdWithProductsQuery(Guid SupplierId):IRequest<SupplierWithProductsResponseDto>;
}
