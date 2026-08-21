using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand>
    {
        private readonly ISupplierWriteRepository _supplierWriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSupplierCommandHandler(
            ISupplierWriteRepository supplierWriteRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteRepository = supplierWriteRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can delete this resources");

            var supplier = await _supplierWriteRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken) ??
                throw new DomainNotFoundException("Supplier not found");

            _supplierWriteRepository.Remove(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
