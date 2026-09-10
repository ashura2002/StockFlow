namespace Infrastructure.Settings
{
    public sealed class FrontendSettings
    {
        public const string SectionName = "Frontend";
        public required string BaseUrl { get; init; }
    }
}
