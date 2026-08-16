
namespace Infrastructure.Settings
{
    public sealed class SeededUserSettings
    {
        public const string SectionName = "SeededUser";
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
