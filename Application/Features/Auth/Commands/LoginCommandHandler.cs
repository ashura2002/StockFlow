using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            IUserWriteRepository userWriteRepository,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            _userWriteRepository = userWriteRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }


        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userWriteRepository.GetUserByEmailAsync(request.Email, cancellationToken) ??
                throw new DomainNotFoundException("User not found.");

            bool isPasswordMatch = _passwordService.Verify(request.Password, user.Password.Value);
            if (!isPasswordMatch) throw new DomainUnauthorizedException("Invalid credentials.");

            return _jwtService.GenerateToken(user);
        }
    }
}
