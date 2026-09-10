
namespace WebAPI.RequestDtos
{
    public sealed record CreateProfileRequest(
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string Address);
}
