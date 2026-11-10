using Application.Common.Errors;
using Application.Common.Results;
using Application.Features.Auth.Responses;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResultWithValue<TokenResponse>>
    {
        private readonly IRefreshTokenReadRepository _refreshTokenReadRepository;
        private readonly IRefreshTokenWriteRepository _refreshTokenWriteRepository;
        private readonly ITokenHasherService _tokenHasherService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IRefreshTokenReadRepository refreshTokenReadRepository,
            IRefreshTokenWriteRepository refreshTokenWriteRepository,
            ITokenHasherService tokenHasherService,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenReadRepository = refreshTokenReadRepository;
            _refreshTokenWriteRepository = refreshTokenWriteRepository;
            _tokenHasherService = tokenHasherService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }


        public async Task<ResultWithValue<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenHash = _tokenHasherService.Hash(request.RefreshToken);

            var refreshToken = await _refreshTokenReadRepository.GetByTokenHashAsync(tokenHash, cancellationToken);
            if (refreshToken is null)
            {
                return ResultWithValue<TokenResponse>.Failure(
                    AuthErrors.RefreshTokenInvalid);
            }

            if (!refreshToken.IsActive())
            {
                return ResultWithValue<TokenResponse>.Failure(AuthErrors.RefreshTokenInvalid);
            }

            if (refreshToken.User is null)
            {
                return ResultWithValue<TokenResponse>.Failure(AuthErrors.RefreshTokenInvalid);
            }

            var user = refreshToken.User;

            var accessToken = _jwtService.GenerateAccessToken(user);
            var rawRefreshToken = _jwtService.GenerateRefreshToken();
            var rawRefreshTokenHash = _tokenHasherService.Hash(rawRefreshToken);

            var newRefreshToken = RefreshToken.Create(user.Id, rawRefreshTokenHash, DateTime.UtcNow.AddDays(7));
            _refreshTokenWriteRepository.Add(newRefreshToken);

            refreshToken.Revoke();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new TokenResponse(accessToken, rawRefreshToken);
            return ResultWithValue<TokenResponse>.Success(result);
        }
    }
}
