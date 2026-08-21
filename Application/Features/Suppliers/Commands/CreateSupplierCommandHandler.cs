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
        private readonly ICurrentUserService _currentUserService;
        private readonly ISupplierReadRepository _supplierReadRepository;

        public CreateSupplierCommandHandler(
            ISupplierWriteRepository supplierWriteRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ISupplierReadRepository supplierReadRepository)
        {
            _supplierWriteRepository = supplierWriteRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _supplierReadRepository = supplierReadRepository;
        }

        public async Task<Guid> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can add a supplier.");

            if (await _supplierReadRepository.IsSupplierEmailExistAsync(request.Email, cancellationToken))
                throw new DomainRuleException("Suplier's email is already exist.");

            var emailVo = EmailVo.Create(request.Email);
            var phoneNumberVo = PhoneNumberVo.Create(request.PhoneNumber);
            var addressVo = AddressVo.Create(request.Address);

            var supplier = Supplier.Create(request.SupplierName, emailVo, phoneNumberVo, addressVo);
            _supplierWriteRepository.Add(supplier); 

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return supplier.Id;
        }
    }

}
