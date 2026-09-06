using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Auth.Commands
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, string>
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
            var user = await _userWriteRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null || !_passwordService.Verify(request.Password, user.Password.Value))
            {
                throw new DomainUnauthorizedException("Invalid email or password.");
            }
            return _jwtService.GenerateToken(user);
        }
    }
}
