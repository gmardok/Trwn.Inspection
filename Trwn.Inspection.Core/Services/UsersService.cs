using Trwn.Inspection.Infrastructure;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Core.Services
{
    public sealed class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public UsersService(IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        private int UserId =>
            _userContext.GetUserId()
            ?? throw new InvalidOperationException("User identity is not available.");

        public Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            return _userRepository.GetByIdAsync(UserId, cancellationToken);
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _userRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User?> UpdateDisplayNameAsync(int userId, string? displayName, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);
            if (user == null)
            {
                return null;
            }

            user.DisplayName = displayName;
            await _userRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);
            return user;
        }
    }
}
