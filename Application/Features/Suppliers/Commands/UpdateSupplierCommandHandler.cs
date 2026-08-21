using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
    {
        private readonly ISupplierWriteRepository _supplierWriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSupplierCommandHandler(
            ISupplierWriteRepository supplierWriteRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteRepository = supplierWriteRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can update a supplier.");

            var email = EmailVo.Create(request.Email);
            var phoneNumber = PhoneNumberVo.Create(request.PhoneNumber);
            var address = AddressVo.Create(request.Address);

            var supplier = await _supplierWriteRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken) ??
                throw new DomainNotFoundException("Supplier not found");

            supplier.UpdateSupplierEmail(email);
            supplier.UpdateSupplierPhoneNumber(phoneNumber);
            supplier.UpdateSupplierAddress(address);
            supplier.UpdateSuplierName(request.SupplierName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
