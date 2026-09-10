namespace WebAPI.RequestDtos
{
    public sealed record CustomerRegistrationRequest(
        string Email,
        string Password,
        string ConfirmPassword);
}
