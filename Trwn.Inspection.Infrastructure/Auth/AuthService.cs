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
    private readonly IUserRepository _userRepository;

    public AuthService(
        InspectionDbContext db,
        IOptions<AuthSettings> authOptions,
        IEmailDomainPolicy emailDomainPolicy,
        IJwtTokenService jwtTokenService,
        ILoginCodeEmailSender emailSender,
        IUserRepository userRepository)
    {
        _db = db;
        _settings = authOptions.Value;
        _emailDomainPolicy = emailDomainPolicy;
        _jwtTokenService = jwtTokenService;
        _emailSender = emailSender;
        _userRepository = userRepository;
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

        var user = await _userRepository.GetOrCreateAsync(email, cancellationToken).ConfigureAwait(false);

        const int maxGuids = 5;
        for (var attempt = 0; attempt < maxGuids; attempt++)
        {
            var segments = Guid.NewGuid().ToString("D").Split('-');
            foreach (var segment in segments)
            {
                var code = segment.ToUpperInvariant();
                var codeExists = await _db.AuthSessions
                    .AnyAsync(s => s.Code == code, cancellationToken)
                    .ConfigureAwait(false);

                if (codeExists)
                    continue;

                var session = new AuthSession
                {
                    Email = email,
                    Code = code,
                    CreatedAtUtc = DateTime.UtcNow,
                    AuthToken = null,
                    TokenExpiresAtUtc = null,
                    IsLoggedOut = false,
                    UserId = user.Id,
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
                    // Race condition: another request claimed this code between our check and save
                    _db.Entry(session).State = EntityState.Detached;
                }
            }
        }

        return new AuthSendCodeResult(false, 500, "Could not allocate a unique code.");
    }

    public async Task<AuthTokenResult> ExchangeCodeForTokenAsync(string? code, CancellationToken cancellationToken)
    {
#if DEBUG
        return await ExchangeCodeForTokenDebugAsync(code, cancellationToken);
#endif
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

        var issued = _jwtTokenService.CreateToken(session.Email, session.Id, session.UserId ?? 0);
        session.AuthToken = issued.Token;
        session.TokenExpiresAtUtc = issued.ExpiresAtUtc;
        session.IsLoggedOut = false;

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new AuthTokenResult(true, 200, null, issued.Token, issued.ExpiresAtUtc);
    }

    public async Task<AuthTokenResult> ExchangeCodeForTokenDebugAsync(string? code, CancellationToken cancellationToken)
    {
        const string debugEmail = "debug@email.hh";
        var debugUser = await _userRepository.GetOrCreateAsync(debugEmail, cancellationToken).ConfigureAwait(false);

        var session = await _db.AuthSessions.FirstOrDefaultAsync(s => s.Code == code);
        if (session == null)
        {
            session = new AuthSession
            {
                Email = debugEmail,
                Code = code ?? "123",
                CreatedAtUtc = DateTime.UtcNow,
                AuthToken = null,
                TokenExpiresAtUtc = null,
                IsLoggedOut = false,
                UserId = debugUser.Id,
            };

            _db.AuthSessions.Add(session);
        }

        await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var issued = _jwtTokenService.CreateToken(session.Email, session.Id, debugUser.Id);
        session.AuthToken = issued.Token;
        session.TokenExpiresAtUtc = issued.ExpiresAtUtc;

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

        var issued = _jwtTokenService.CreateToken(session.Email, session.Id, session.UserId ?? 0);
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
