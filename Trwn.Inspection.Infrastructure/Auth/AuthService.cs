using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Data;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure.Auth;

public sealed class AuthService : IAuthService
{
    private readonly InspectionDbContext _db;
    private readonly AuthSettings _settings;
    private readonly IEmailDomainPolicy _emailDomainPolicy;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILoginCodeEmailSender _emailSender;

    public AuthService(
        InspectionDbContext db,
        IOptions<AuthSettings> authOptions,
        IEmailDomainPolicy emailDomainPolicy,
        IJwtTokenService jwtTokenService,
        ILoginCodeEmailSender emailSender)
    {
        _db = db;
        _settings = authOptions.Value;
        _emailDomainPolicy = emailDomainPolicy;
        _jwtTokenService = jwtTokenService;
        _emailSender = emailSender;
    }

    public async Task<AuthSendCodeResult> SendLoginCodeAsync(string? email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return new AuthSendCodeResult(false, 400, "Email is required.");
        }

        email = email.Trim();
        if (!MailboxAddress.TryParse(email, out _))
        {
            return new AuthSendCodeResult(false, 400, "Email is not valid.");
        }

        if (!_emailDomainPolicy.IsDomainAllowed(email))
        {
            return new AuthSendCodeResult(false, 403, "Email domain is not allowed.");
        }

        const int maxAttempts = 5;
        for (var attempt = 0; attempt < maxAttempts; attempt++)
        {
            var code = Guid.NewGuid().ToString("D").Split('-')[0].ToUpperInvariant();
            var session = new AuthSession
            {
                Email = email,
                Code = code,
                CreatedAtUtc = DateTime.UtcNow,
                AuthToken = null,
                TokenExpiresAtUtc = null,
                IsLoggedOut = false,
            };

            _db.AuthSessions.Add(session);

            try
            {
                await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                await _emailSender.SendSignInCodeAsync(email, code, cancellationToken).ConfigureAwait(false);
                return new AuthSendCodeResult(true, 200, null);
            }
            catch (DbUpdateException)
            {
                _db.Entry(session).State = EntityState.Detached;
            }
        }

        return new AuthSendCodeResult(false, 500, "Could not allocate a unique code.");
    }

    public async Task<AuthTokenResult> ExchangeCodeForTokenAsync(string? code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return new AuthTokenResult(false, 400, "Code is required.", null, null);
        }

        var normalized = code.Trim().ToUpperInvariant();
        var session = await _db.AuthSessions
            .FirstOrDefaultAsync(s => s.Code == normalized, cancellationToken)
            .ConfigureAwait(false);

        if (session == null)
        {
            return new AuthTokenResult(false, 404, "Code not found.", null, null);
        }

        var expiresBoundary = session.CreatedAtUtc.AddMinutes(_settings.CodeExpirationMinutes);
        if (DateTime.UtcNow > expiresBoundary)
        {
            return new AuthTokenResult(false, 400, "Code has expired.", null, null);
        }

        var issued = _jwtTokenService.CreateToken(session.Email, session.Id);
        session.AuthToken = issued.Token;
        session.TokenExpiresAtUtc = issued.ExpiresAtUtc;
        session.IsLoggedOut = false;

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new AuthTokenResult(true, 200, null, issued.Token, issued.ExpiresAtUtc);
    }

    public async Task<AuthTokenResult> RefreshTokenAsync(string? token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthTokenResult(false, 400, "Token is required.", null, null);
        }

        var principal = _jwtTokenService.ValidatePrincipal(token, validateLifetime: false);
        if (principal == null || !_jwtTokenService.TryGetSessionId(principal, out var sessionId))
        {
            return new AuthTokenResult(false, 401, "Token is invalid.", null, null);
        }

        var session = await _db.AuthSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken)
            .ConfigureAwait(false);

        if (session == null || session.IsLoggedOut || session.AuthToken != token)
        {
            return new AuthTokenResult(false, 401, "Token is invalid or revoked.", null, null);
        }

        var issued = _jwtTokenService.CreateToken(session.Email, session.Id);
        session.AuthToken = issued.Token;
        session.TokenExpiresAtUtc = issued.ExpiresAtUtc;

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new AuthTokenResult(true, 200, null, issued.Token, issued.ExpiresAtUtc);
    }

    public async Task<AuthLogoutResult> LogoutAsync(string? token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthLogoutResult(false, 400, "Token is required.");
        }

        var session = await _db.AuthSessions
            .FirstOrDefaultAsync(s => s.AuthToken == token, cancellationToken)
            .ConfigureAwait(false);

        if (session == null)
        {
            var principal = _jwtTokenService.ValidatePrincipal(token, validateLifetime: false);
            if (principal != null && _jwtTokenService.TryGetSessionId(principal, out var sessionId))
            {
                session = await _db.AuthSessions
                    .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        if (session == null)
        {
            return new AuthLogoutResult(false, 404, "Session not found.");
        }

        session.IsLoggedOut = true;
        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new AuthLogoutResult(true, 200, null);
    }
}
