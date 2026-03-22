namespace Trwn.Inspection.Infrastructure.Auth;

public interface IAuthService
{
    Task<AuthSendCodeResult> SendLoginCodeAsync(string? email, CancellationToken cancellationToken);

    Task<AuthTokenResult> ExchangeCodeForTokenAsync(string? code, CancellationToken cancellationToken);

    Task<AuthTokenResult> RefreshTokenAsync(string? token, CancellationToken cancellationToken);

    Task<AuthLogoutResult> LogoutAsync(string? token, CancellationToken cancellationToken);
}

public sealed record AuthSendCodeResult(bool Success, int StatusCode, string? ErrorMessage);

public sealed record AuthTokenResult(bool Success, int StatusCode, string? ErrorMessage, string? Token, DateTime? ExpiresAtUtc);

public sealed record AuthLogoutResult(bool Success, int StatusCode, string? ErrorMessage);
