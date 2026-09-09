namespace WebAPI.RequestDtos
{
    public sealed record ConfirmAccountRestoreRequest
    {
        public required string Email { get; set; }
        public required string VerificationCode { get; set; }
        
    }
}
