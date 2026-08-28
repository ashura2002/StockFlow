using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Suppliers.Commands
{
    public sealed class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Guid>
    {
        private readonly ISupplierWriteRepository _supplierWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierReadRepository _supplierReadRepository;

        public CreateSupplierCommandHandler(
            ISupplierWriteRepository supplierWriteRepository,
            IUnitOfWork unitOfWork,
            ISupplierReadRepository supplierReadRepository)
        {
            _supplierWriteRepository = supplierWriteRepository;
            _unitOfWork = unitOfWork;
            _supplierReadRepository = supplierReadRepository;
        }

        public async Task<Guid> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var emailVo = EmailVo.Create(request.Email);
            var phoneNumberVo = PhoneNumberVo.Create(request.PhoneNumber);
            var addressVo = AddressVo.Create(request.Address);

            if (await _supplierReadRepository.IsSupplierEmailExistAsync(request.Email, null, cancellationToken))
                throw new DomainConflictException("Supplier's email is already exist.");

            var supplier = Supplier.Create(request.SupplierName, emailVo, phoneNumberVo, addressVo);
            _supplierWriteRepository.Add(supplier); 

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return supplier.Id;
        }
    }

}
