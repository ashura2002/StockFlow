using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
    {
        private readonly ISupplierWriteRepository _supplierWriteRepository;
        private readonly ISupplierReadRepository _supplierReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSupplierCommandHandler(
            ISupplierWriteRepository supplierWriteRepository,
            ISupplierReadRepository supplierReadRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteRepository = supplierWriteRepository;
            _supplierReadRepository = supplierReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierWriteRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken) ??
                throw new DomainNotFoundException("Supplier not found");

            var email = EmailVo.Create(request.Email);
            var phoneNumber = PhoneNumberVo.Create(request.PhoneNumber);
            var address = AddressVo.Create(request.Address);

            if (await _supplierReadRepository.IsSupplierEmailExistAsync(email.Value, supplier.Id, cancellationToken))
                    throw new DomainConflictException("Supplier email already existed.");

            supplier.UpdateSupplierEmail(email);
            supplier.UpdateSupplierPhoneNumber(phoneNumber);
            supplier.UpdateSupplierAddress(address);
            supplier.UpdateSuplierName(request.SupplierName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
