using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Trwn.Inspection.Web.Infrastructure;

public sealed class HttpAuthSessionContext : IAuthSessionContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpAuthSessionContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetSessionId()
    {
        var sub = _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(sub, out var id) ? id : null;
    }
}
