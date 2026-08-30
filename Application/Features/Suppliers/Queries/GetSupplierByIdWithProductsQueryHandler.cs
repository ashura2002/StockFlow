using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public sealed class GetSupplierByIdWithProductsQueryHandler : IRequestHandler<GetSupplierByIdWithProductsQuery, SupplierWithProductsResponseDto>
    {
        private readonly ISupplierReadRepository _supplierReadRepository;
        public GetSupplierByIdWithProductsQueryHandler(ISupplierReadRepository supplierReadRepository)
        {
            _supplierReadRepository = supplierReadRepository;
        }
        public async Task<SupplierWithProductsResponseDto> Handle(GetSupplierByIdWithProductsQuery request, CancellationToken cancellationToken)
        {
            return await _supplierReadRepository.GetSupplierByIdWithProductsAsync(request.SupplierId, cancellationToken) ??
                throw new DomainNotFoundException("Supplier not found.");
        }
    }
}
