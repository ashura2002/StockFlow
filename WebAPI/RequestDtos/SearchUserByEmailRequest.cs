

namespace WebAPI.RequestDtos
{
    public sealed record SearchUserByEmailRequest(
        string Email,
        int Page = 1,
        int PageSize = 10);
}
