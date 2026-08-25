using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand>
    {
        private readonly ISupplierWriteRepository _supplierWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSupplierCommandHandler(
            ISupplierWriteRepository supplierWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteRepository = supplierWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierWriteRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken) ??
                throw new DomainNotFoundException("Supplier not found");

            _supplierWriteRepository.Remove(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
