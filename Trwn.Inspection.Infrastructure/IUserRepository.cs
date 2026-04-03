using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>Returns the existing User for the given email, or creates one. Thread-safe upsert.</summary>
        Task<User> GetOrCreateAsync(string email, CancellationToken cancellationToken);

        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);

        Task UpdateAsync(User user, CancellationToken cancellationToken);
    }
}
