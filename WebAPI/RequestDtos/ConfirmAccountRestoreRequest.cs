namespace WebAPI.RequestDtos
{
    public sealed record ConfirmAccountRestoreRequest(string Email, string VerificationCode);

}
