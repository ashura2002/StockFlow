namespace WebAPI.RequestDtos
{
    public sealed record ResetPasswordRequest(
        string RawToken,
        string NewPassword);
}
