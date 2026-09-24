using Application.Features.Auth.Responses;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed record RefreshTokenCommand(string RefreshToken): IRequest<TokenResponse>;
}
