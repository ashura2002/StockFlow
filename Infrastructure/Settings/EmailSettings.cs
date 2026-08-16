namespace Infrastructure.Settings
{
    public sealed class EmailSettings
    {
        public const string SectionName = "Email";
        public required string ApiKey { get; set; }
        public required string From { get; set; }
    }
}
