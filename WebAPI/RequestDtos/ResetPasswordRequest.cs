namespace WebAPI.RequestDtos
{
    public sealed record ResetPasswordRequest
    {
        public required string RawToken { get; set; }
        public required string NewPassword { get; set; }
    }
}
