using System.Security.Claims;

namespace Trwn.Inspection.Infrastructure.Auth;

public interface IJwtTokenService
{
    JwtTokenIssueResult CreateToken(string email, int sessionId, int userId);

    ClaimsPrincipal? ValidatePrincipal(string token, bool validateLifetime);

    bool TryGetSessionId(ClaimsPrincipal principal, out int sessionId);

    bool TryGetUserId(ClaimsPrincipal principal, out int userId);
}

public sealed record JwtTokenIssueResult(string Token, DateTime ExpiresAtUtc);
