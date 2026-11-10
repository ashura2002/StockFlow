using Application.Common.Results;
using Application.Features.Auth.Responses;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<ResultWithValue<TokenResponse>>;
}
