using Application.Features.Auth.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
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


        public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenHash = _tokenHasherService.Hash(request.RefreshToken);

            var refreshToken = await _refreshTokenReadRepository.GetByTokenHashAsync(tokenHash, cancellationToken)??
                throw new DomainNotFoundException("Token not found.");

            if (!refreshToken.IsActive())
                throw new DomainUnauthorizedException("Refresh token is expired or revoked.");

            var user = refreshToken.User??
                throw new DomainNotFoundException("User not found.");

            var accessToken = _jwtService.GenerateAccessToken(user);
            var rawRefreshToken = _jwtService.GenerateRefreshToken();
            var rawRefreshTokenHash = _tokenHasherService.Hash(rawRefreshToken);

            var newRefreshToken = RefreshToken.Create(user.Id, rawRefreshTokenHash, DateTime.UtcNow.AddDays(7));
            _refreshTokenWriteRepository.Add(newRefreshToken);

            refreshToken.Revoke();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TokenResponse(accessToken, rawRefreshToken);
        }
    }
}
