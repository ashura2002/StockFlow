
namespace WebAPI.RequestDtos
{
    public sealed record CreateProfileRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string Address { get; set; }
    }
}
