using IdentitySystem.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentitySystem.Stores
{
    public class InMemoryUserStore<TUser> : IUserStore<TUser> where TUser : class, IApplicationUser<string>, new()
    {
        private readonly ILookupNormalizer lookupNormalizer;

        private readonly ConcurrentDictionary<string, TUser> users;

        public InMemoryUserStore(ILookupNormalizer lookupNormalizer)
        {
            this.lookupNormalizer = lookupNormalizer;
            this.users = new ConcurrentDictionary<string, TUser>();

            var name = "Ashley";
            var id = Guid.NewGuid().ToString();
            this.users.TryAdd(id, new TUser
            {
                ID = id,
                UserName = name,
                NormalizedUserName = this.lookupNormalizer.NormalizeName(name),
            });;
        }

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            if (!this.users.ContainsKey(user.ID) && this.users.TryAdd(user.ID, user))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(this.CreateIdentityError());
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            if (this.users.ContainsKey(user.ID) && this.users.TryRemove(user.ID, out user))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(this.CreateIdentityError());
        }

        public void Dispose()
        { }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userId);

            return Task.FromResult(this.users[userId]);
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(this.users.Values.FirstOrDefault(user => user.NormalizedUserName == normalizedUserName));
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.ID);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedUserName = normalizedName);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.UserName = userName);
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            if (this.users.ContainsKey(user.ID) && this.users.TryUpdate(user.ID, user, user))
            {
                return Task.FromResult(this.CreateIdentityError());
            }

            return Task.FromResult(this.CreateIdentityError());
        }

        private IdentityResult CreateIdentityError()
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "Storage Failure",
                Description = "User Store Error"
            });
        }
    }
}