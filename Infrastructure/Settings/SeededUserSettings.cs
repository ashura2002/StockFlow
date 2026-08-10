using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Settings
{
    public class SeededUserSettings
    {
        public const string SectionName = "SeededUser";
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
