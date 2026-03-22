public interface IAuthSessionContext
{
    /// <summary>Auth session id from JWT (nameidentifier claim) when authenticated.</summary>
    int? GetSessionId();
}
