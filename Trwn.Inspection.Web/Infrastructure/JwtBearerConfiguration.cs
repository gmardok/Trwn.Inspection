using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Trwn.Inspection.Configuration;
using Trwn.Inspection.Data;

namespace Trwn.Inspection.Web.Infrastructure;

public static class JwtBearerConfiguration
{
    public static void ConfigureJwtBearer(JwtBearerOptions options, AuthSettings auth)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(auth.JwtSecretKey)),
            ValidateIssuer = true,
            ValidIssuer = auth.JwtIssuer,
            ValidateAudience = true,
            ValidAudience = auth.JwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = ClaimTypes.NameIdentifier,
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var authHeader = context.Request.Headers.Authorization.ToString();
                if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Fail(new SecurityTokenValidationException("Missing bearer token."));
                    return;
                }

                var rawToken = authHeader["Bearer ".Length..].Trim();
                var sessionIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(sessionIdClaim, out var sessionId))
                {
                    context.Fail(new SecurityTokenValidationException("Invalid session claim."));
                    return;
                }

                await using var scope = context.HttpContext.RequestServices.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<InspectionDbContext>();
                var session = await db.AuthSessions.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == sessionId 
                    || (!string.IsNullOrEmpty(s.AuthToken)) && s.AuthToken.Equals(rawToken),
                    context.HttpContext.RequestAborted)
                    .ConfigureAwait(false);

                if (session == null || session.IsLoggedOut || session.AuthToken != rawToken)
                {
                    context.Fail(new SecurityTokenValidationException("Session revoked or token mismatch."));
                }
            },
        };
    }
}
