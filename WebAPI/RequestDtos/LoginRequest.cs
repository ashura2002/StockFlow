

namespace WebAPI.RequestDtos
{
    public sealed record LoginRequest(
        string Email,
        string Password);

}
