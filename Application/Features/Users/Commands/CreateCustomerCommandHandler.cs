using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands
{
    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResultWithValue<Guid>>
    {
        private readonly IUserWriteRepository _userWrite;
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public CreateCustomerCommandHandler(
            IUserWriteRepository userWrite,
            IUserReadRepository userRead,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _userWrite = userWrite;
            _userRead = userRead;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<ResultWithValue<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var emailVo = EmailVo.Create(request.Email);
            if (await _userRead.IsEmailExistAsync(emailVo.Value, cancellationToken))
                return ResultWithValue<Guid>.Failure(UserErrors.EmailAlreadyInUsed);


            var passwordVo = PasswordVo.Create(request.Password);
            var hashPassword = _passwordService.HashPassword(passwordVo.Value);

            var user = User.Create(emailVo, Role.Customer, PasswordVo.Create(hashPassword));

            _userWrite.Add(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ResultWithValue<Guid>.Success(user.Id);
        }
    }
}
