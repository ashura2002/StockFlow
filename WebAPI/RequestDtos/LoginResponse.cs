namespace WebAPI.RequestDtos
{
    public sealed record LoginResponse(string AccessToken, string RefreshToken);
}
