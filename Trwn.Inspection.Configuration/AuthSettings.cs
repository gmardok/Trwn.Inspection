namespace Trwn.Inspection.Configuration
{
    public class AuthSettings
    {
        /// <summary>Allowed email address domains (part after @), case-insensitive.</summary>
        public string[] EmailWhitelistDomains { get; set; } = System.Array.Empty<string>();

        public int CodeExpirationMinutes { get; set; } = 15;

        public int JwtExpirationHours { get; set; } = 24;

        /// <summary>Symmetric key for HS256; must be sufficiently long (e.g. 32+ random bytes as Base64).</summary>
        public string JwtSecretKey { get; set; } = null!;

        public string JwtIssuer { get; set; } = "Trwn.Inspection";

        public string JwtAudience { get; set; } = "Trwn.Inspection";

        public SmtpSettings Smtp { get; set; } = new SmtpSettings();
    }

    public class SmtpSettings
    {
        /// <summary>e.g. smtp.gmail.com</summary>
        public string Host { get; set; } = "smtp.gmail.com";

        public int Port { get; set; } = 587;

        public string UserName { get; set; } = null!;

        /// <summary>For Gmail use an app password, not the account password.</summary>
        public string Password { get; set; } = null!;

        public string FromAddress { get; set; } = null!;

        public string FromDisplayName { get; set; } = "Trwn Inspection";

        public bool UseStartTls { get; set; } = true;
    }
}
