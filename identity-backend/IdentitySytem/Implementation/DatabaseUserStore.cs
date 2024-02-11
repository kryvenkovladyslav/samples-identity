using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentitySystem.Implementation
{
    public class DatabaseUserStore<TUser, TKey, TContext> : IUserStore<TUser>, IQueryableUserStore<TUser>
        where TKey : IEquatable<TKey>
        where TContext: DbContext
        where TUser: BaseApplicationUser<TKey>, new()

    {
        private bool disposed;

        private readonly TContext context;

        public IQueryable<TUser> Users
        {
            get
            {
                return this.GetSet();
            }
        }

        public DatabaseUserStore(TContext context) 
        {
            this.disposed = false;
            this.context = context;
        }

        public virtual async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            this.ThrowIfDisposed();

            this.GetSet().Add(user);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async virtual Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            this.ThrowIfDisposed();
            
            this.GetSet().Remove(user);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public virtual Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            var id = this.ConvertIdentifierFromString(userId);
            return this.GetSet().FindAsync(new object[] { id }, cancellationToken).AsTask();
        }

        public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedUserName);
            this.ThrowIfDisposed();

            return this.GetSet().FirstOrDefaultAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(this.ConvertIdentifierToString(user.ID));
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedName, nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(userName, nameof(userName));

            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            this.GetSet().Update(user);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            if(this.disposed)
            {
                return;
            }

            this.context.Dispose();
            this.disposed = true;
        }

        protected virtual TKey ConvertIdentifierFromString(string id)
        {
            if (id == null)
            {
                return default(TKey);
            }

            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }

        protected virtual string ConvertIdentifierToString(TKey id)
        {
            return TypeDescriptor.GetConverter(typeof(string)).ConvertToInvariantString(id);
        }

        protected async virtual Task SaveChangesAsync(CancellationToken token = default)
        {
            await this.context.SaveChangesAsync(token);
        }

        protected virtual void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);

            }
        }

        private DbSet<TUser> GetSet()
        {
            return this.context.Set<TUser>();
        }
    }
}