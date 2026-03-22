using System.Security.Claims;

namespace Trwn.Inspection.Infrastructure.Auth;

public interface IJwtTokenService
{
    JwtTokenIssueResult CreateToken(string email, int sessionId);

    ClaimsPrincipal? ValidatePrincipal(string token, bool validateLifetime);

    bool TryGetSessionId(ClaimsPrincipal principal, out int sessionId);
}

public sealed record JwtTokenIssueResult(string Token, DateTime ExpiresAtUtc);
