using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trwn.Inspection.Infrastructure.Auth;

namespace Trwn.Inspection.Web.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Requests a one-time sign-in code by email (whitelisted domains only).</summary>
    [HttpGet(nameof(GetCode))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCode([FromQuery] string? email, CancellationToken cancellationToken)
    {
        var result = await _authService.SendLoginCodeAsync(email, cancellationToken).ConfigureAwait(false);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }

        return Ok();
    }

    /// <summary>Exchanges the email code for a JWT.</summary>
    [HttpPost(nameof(GetToken))]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetToken([FromBody] AuthCodeRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ExchangeCodeForTokenAsync(request?.Code, cancellationToken).ConfigureAwait(false);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }

        return Ok(new AuthTokenResponse
        {
            Token = result.Token!,
            ExpiresAtUtc = result.ExpiresAtUtc!.Value,
        });
    }

    /// <summary>Issues a new JWT with a fresh expiry (previous token is invalidated).</summary>
    [HttpPost(nameof(RefreshToken))]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] AuthTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(request?.Token, cancellationToken).ConfigureAwait(false);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }

        return Ok(new AuthTokenResponse
        {
            Token = result.Token!,
            ExpiresAtUtc = result.ExpiresAtUtc!.Value,
        });
    }

    /// <summary>Marks the session as logged out; the token can no longer be refreshed.</summary>
    [HttpPost(nameof(Logout))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Logout([FromBody] AuthTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogoutAsync(request?.Token, cancellationToken).ConfigureAwait(false);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }

        return Ok();
    }
}

public sealed class AuthCodeRequest
{
    public string? Code { get; set; }
}

public sealed class AuthTokenRequest
{
    public string? Token { get; set; }
}

public sealed class AuthTokenResponse
{
    public string Token { get; set; } = null!;

    public DateTime ExpiresAtUtc { get; set; }
}
