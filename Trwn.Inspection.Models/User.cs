using System;
using System.Collections.Generic;

namespace Trwn.Inspection.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreatedAtUtc { get; set; }
        public string? DisplayName { get; set; }

        public ICollection<AuthSession> AuthSessions { get; set; } = new List<AuthSession>();
        public ICollection<InspectionReport> InspectionReports { get; set; } = new List<InspectionReport>();
    }
}
