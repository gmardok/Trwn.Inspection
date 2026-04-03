using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trwn.Inspection.Data;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Infrastructure.Repositories
{
    public sealed class UserSqlRepository : IUserRepository
    {
        private readonly InspectionDbContext _db;

        public UserSqlRepository(InspectionDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetOrCreateAsync(string email, CancellationToken cancellationToken)
        {
            var existing = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                .ConfigureAwait(false);

            if (existing != null)
            {
                return existing;
            }

            var user = new User { Email = email, CreatedAtUtc = DateTime.UtcNow };
            _db.Users.Add(user);

            try
            {
                await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return user;
            }
            catch (DbUpdateException)
            {
                // Unique constraint race — another request created the row first.
                _db.Entry(user).State = EntityState.Detached;
                return await _db.Users
                    .FirstAsync(u => u.Email == email, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.Users
                .OrderBy(u => u.Email)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
