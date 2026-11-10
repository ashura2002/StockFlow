using Application.Common.Errors;
using Application.Common.Results;
using Application.Features.Auth.Responses;
using Application.Interfaces;
using Domain.Entities;
using MediatR;


namespace Application.Features.Auth.Commands
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ResultWithValue<TokenResponse>>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ITokenHasherService _tokenHasherService;
        private readonly IRefreshTokenWriteRepository _refreshTokenWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserWriteRepository userWriteRepository,
            IPasswordService passwordService,
            IJwtService jwtService,
            ITokenHasherService tokenHasherService,
            IRefreshTokenWriteRepository refreshTokenWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _userWriteRepository = userWriteRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _tokenHasherService = tokenHasherService;
            _refreshTokenWriteRepository = refreshTokenWriteRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<ResultWithValue<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userWriteRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null || !_passwordService.Verify(request.Password, user.Password.Value))
                return ResultWithValue<TokenResponse>.Failure(AuthErrors.InvalidCredentials);

            var accessToken = _jwtService.GenerateAccessToken(user);
            var rawRefreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenHash = _tokenHasherService.Hash(rawRefreshToken);

            var refreshToken = RefreshToken.Create(user.Id, refreshTokenHash, DateTime.UtcNow.AddDays(7));
            _refreshTokenWriteRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new TokenResponse(accessToken, rawRefreshToken);
            return ResultWithValue<TokenResponse>.Success(result);
        }
    }
}
