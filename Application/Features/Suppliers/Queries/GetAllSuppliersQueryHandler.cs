using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public sealed class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IReadOnlyCollection<SupplierResponseDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ISupplierReadRepository _supplierReadRepository;

        public GetAllSuppliersQueryHandler(
            ICurrentUserService currentUserService,
            ISupplierReadRepository supplierReadRepository)
        {
            _currentUserService = currentUserService;
            _supplierReadRepository = supplierReadRepository;
        }


        public async Task<IReadOnlyCollection<SupplierResponseDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can view all suppliers");

            return await _supplierReadRepository.GetAllSuppliersAsync(request.Page, request.PageSize, cancellationToken);
        }
    }
}
