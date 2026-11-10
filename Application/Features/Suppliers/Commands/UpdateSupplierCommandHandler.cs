using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Result>
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

        public async Task<Result> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierWriteRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken);
            if (supplier is null)
                return Result.Failure(SupplierErrors.SupplierNotFound);

            var email = EmailVo.Create(request.Email);
            var phoneNumber = PhoneNumberVo.Create(request.PhoneNumber);
            var address = AddressVo.Create(request.Address);

            var isSupplierEmailExist = await _supplierReadRepository.IsSupplierEmailExistAsync(email.Value, supplier.Id, cancellationToken);
            if (isSupplierEmailExist)
                return Result.Failure(SupplierErrors.SuplierEmailConflict);

            supplier.UpdateSupplierEmail(email);
            supplier.UpdateSupplierPhoneNumber(phoneNumber);
            supplier.UpdateSupplierAddress(address);
            supplier.UpdateSuplierName(request.SupplierName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
