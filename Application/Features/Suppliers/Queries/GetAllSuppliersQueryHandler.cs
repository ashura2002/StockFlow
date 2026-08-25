using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public sealed class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IReadOnlyCollection<SupplierResponseDto>>
    {
        private readonly ISupplierReadRepository _supplierReadRepository;

        public GetAllSuppliersQueryHandler(
            ISupplierReadRepository supplierReadRepository)
        {
            _supplierReadRepository = supplierReadRepository;
        }


        public async Task<IReadOnlyCollection<SupplierResponseDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            return await _supplierReadRepository.GetAllSuppliersAsync(request.Page, request.PageSize, cancellationToken);
        }
    }
}
