using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly IUserWriteRepository _userWrite;
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(
            IUserWriteRepository userWrite,
            IUserReadRepository userRead,
            IUnitOfWork unitOfWork)
        {
            _userWrite = userWrite;
            _userRead = userRead;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var emailVo = EmailVo.Create(request.Email);
            var passwordVo = PasswordVo.Create(request.Email);

             if(await _userRead.IsEmailExistAsync(emailVo.Value, cancellationToken))
                     throw new DomainBadRequestException("Email already exist.");

            var user = User.Create(emailVo, Role.Customer, passwordVo);

            _userWrite.Add(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}
