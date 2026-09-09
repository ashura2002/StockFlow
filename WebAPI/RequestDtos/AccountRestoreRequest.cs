namespace WebAPI.RequestDtos
{
    public sealed record AccountRestoreRequest
    {
        public required string Email { get; set; }
    }
}
