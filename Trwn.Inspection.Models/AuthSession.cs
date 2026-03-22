using System;

namespace Trwn.Inspection.Models
{
    /// <summary>
    /// Email login session: one-time code exchange for JWT, then refresh/logout tracking.
    /// </summary>
    public class AuthSession
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        /// <summary>First segment of a GUID (or similar short code) used for email verification.</summary>
        public string Code { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }

        /// <summary>JWT bearer token; set after successful <c>GetToken</c>.</summary>
        public string? AuthToken { get; set; }

        public DateTime? TokenExpiresAtUtc { get; set; }

        public bool IsLoggedOut { get; set; }
    }
}
