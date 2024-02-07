using IdentitySystem.Abstract;
using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using System.Threading.Tasks;

namespace IdentitySystem.Stores
{
    public class UserStore<TUser> : IUserStore<TUser> where TUser : class, IApplicationUser<string>
    {
        private readonly ConcurrentDictionary<string, TUser> users;

        public UserStore()
        {
            this.users = new ConcurrentDictionary<string, TUser>();
        }

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            if(!this.users.ContainsKey(user.ID) && this.users.TryAdd(user.ID, user))
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
        {
            throw new NotImplementedException();
        }

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

            return Task.FromResult(user.ID.ToString());
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