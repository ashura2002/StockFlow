

namespace Application.Features.Auth.Responses
{
    public sealed record TokenResponse(string AccessToken, string RefreshToken);
}
