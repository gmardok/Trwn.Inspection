using System;
using System.Collections.Generic;

namespace Trwn.Inspection.WebClient
{
    /// <summary>
    /// Configuration options for the Inspection Reports Web API client
    /// </summary>
    public class InspectionReportsClientOptions
    {
        /// <summary>
        /// The base URL of the Inspection Reports Web API
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Request timeout in seconds (default: 30 seconds)
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// API key for authentication (if required)
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Additional headers to include in requests
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}