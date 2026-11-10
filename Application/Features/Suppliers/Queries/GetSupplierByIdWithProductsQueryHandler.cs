using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public sealed class GetSupplierByIdWithProductsQueryHandler : IRequestHandler<GetSupplierByIdWithProductsQuery,ResultWithValue< SupplierWithProductsResponseDto>>
    {
        private readonly ISupplierReadRepository _supplierReadRepository;
        public GetSupplierByIdWithProductsQueryHandler(ISupplierReadRepository supplierReadRepository)
        {
            _supplierReadRepository = supplierReadRepository;
        }
        public async Task<ResultWithValue<SupplierWithProductsResponseDto>> Handle(GetSupplierByIdWithProductsQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierReadRepository.GetSupplierByIdWithProductsAsync(request.SupplierId, cancellationToken);
            if (supplier is null)
                return ResultWithValue<SupplierWithProductsResponseDto>.Failure(SupplierErrors.SupplierNotFound);

            return ResultWithValue<SupplierWithProductsResponseDto>.Success(supplier);

        }
    }
}
