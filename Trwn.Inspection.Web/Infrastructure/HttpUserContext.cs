using Trwn.Inspection.Core;

namespace Trwn.Inspection.Web.Infrastructure;

public sealed class HttpUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetUserId()
    {
        var val = _httpContextAccessor.HttpContext?.User.FindFirst("uid")?.Value;
        return int.TryParse(val, out var id) ? id : null;
    }
}
