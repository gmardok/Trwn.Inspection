using Microsoft.Extensions.Options;
using Trwn.Inspection.Configuration;

namespace Trwn.Inspection.Infrastructure.Auth;

public sealed class EmailDomainPolicy : IEmailDomainPolicy
{
    private readonly IOptions<AuthSettings> _options;

    public EmailDomainPolicy(IOptions<AuthSettings> options)
    {
        _options = options;
    }

    public bool IsDomainAllowed(string emailAddress)
    {
        var whitelist = _options.Value.EmailWhitelistDomains;
        if (whitelist?.Length == 1 && whitelist[0].Equals("*"))
        {
            return true; // Allow all domains if wildcard is present
        }

        if (whitelist == null || whitelist.Length == 0)
        {
            return false;
        }

        var at = emailAddress.LastIndexOf('@');
        if (at <= 0 || at == emailAddress.Length - 1)
        {
            return false;
        }

        var domain = emailAddress[(at + 1)..].Trim().ToLowerInvariant();

        foreach (var allowed in whitelist)
        {
            if (string.IsNullOrWhiteSpace(allowed))
            {
                continue;
            }

            if (string.Equals(domain, allowed.Trim().ToLowerInvariant(), StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
