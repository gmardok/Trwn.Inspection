using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Trwn.Inspection.Configuration;

namespace Trwn.Inspection.Infrastructure.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly AuthSettings _settings;

    public JwtTokenService(IOptions<AuthSettings> options)
    {
        _settings = options.Value;
    }

    public JwtTokenIssueResult CreateToken(string email, int sessionId)
    {
        var key = GetSigningKey();
        var expires = DateTime.UtcNow.AddHours(_settings.JwtExpirationHours);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, sessionId.ToString(CultureInfo.InvariantCulture)),
        };

        var jwt = new JwtSecurityToken(
            issuer: _settings.JwtIssuer,
            audience: _settings.JwtAudience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new JwtTokenIssueResult(token, expires);
    }

    public ClaimsPrincipal? ValidatePrincipal(string token, bool validateLifetime)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = GetSigningKey();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _settings.JwtIssuer,
            ValidateAudience = true,
            ValidAudience = _settings.JwtAudience,
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.FromMinutes(1),
        };

        try
        {
            return handler.ValidateToken(token, parameters, out _);
        }
        catch (SecurityTokenException)
        {
            return null;
        }
    }

    public bool TryGetSessionId(ClaimsPrincipal principal, out int sessionId)
    {
        sessionId = 0;
        var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return id != null && int.TryParse(id, CultureInfo.InvariantCulture, out sessionId);
    }

    private SymmetricSecurityKey GetSigningKey()
    {
        var bytes = Encoding.UTF8.GetBytes(_settings.JwtSecretKey);
        if (bytes.Length < 32)
        {
            throw new InvalidOperationException(
                "Auth:JwtSecretKey must be at least 32 bytes when UTF-8 encoded (use a long random secret).");
        }

        return new SymmetricSecurityKey(bytes);
    }
}
