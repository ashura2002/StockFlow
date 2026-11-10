using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Result>
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

        public async Task<Result> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierWriteRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken);
            if (supplier is null)
                return Result.Failure(SupplierErrors.SupplierNotFound);


            _supplierWriteRepository.Remove(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
