using Trwn.Inspection.Models;

public interface IUsersService
{
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<User?> UpdateDisplayNameAsync(int userId, string? displayName, CancellationToken cancellationToken);
}
