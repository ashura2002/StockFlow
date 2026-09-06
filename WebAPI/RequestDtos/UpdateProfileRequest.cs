

namespace WebAPI.RequestDtos
{
    public sealed record UpdateProfileRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }

    }
}
