namespace WebAPI.RequestDtos
{
    public sealed record CustomerRegistrationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
