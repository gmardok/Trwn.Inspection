using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trwn.Inspection.Core;

namespace Trwn.Inspection.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public sealed class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IUserContext _userContext;

    public UsersController(IUsersService usersService, IUserContext userContext)
    {
        _usersService = usersService;
        _userContext = userContext;
    }

    /// <summary>Returns the profile of the currently authenticated user.</summary>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var user = await _usersService.GetCurrentUserAsync(cancellationToken).ConfigureAwait(false);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserResponse(user.Id, user.Email, user.DisplayName, user.CreatedAtUtc));
    }

    /// <summary>Updates the display name of the currently authenticated user.</summary>
    [HttpPut("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _usersService.UpdateDisplayNameAsync(userId.Value, request.DisplayName, cancellationToken).ConfigureAwait(false);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserResponse(user.Id, user.Email, user.DisplayName, user.CreatedAtUtc));
    }
}

public sealed record UserResponse(int Id, string Email, string? DisplayName, DateTime CreatedAtUtc);

public sealed class UpdateUserRequest
{
    public string? DisplayName { get; set; }
}
