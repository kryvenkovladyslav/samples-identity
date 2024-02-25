using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Abstract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.System.Implementation
{
    public class DatabaseRoleStore<TRole, TKey, TContext> : DatabaseStore<TContext>, IRoleStore<TRole>, IQueryableRoleStore<TRole>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TRole : IdentitySystemRole<TKey>
    {
        public IQueryable<TRole> Roles
        {
            get
            {
                return this.GetSet<TRole>();
            }
        }

        public DatabaseRoleStore(TContext context) : base(context) { }

        #region IRoleStore Implementation

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            this.GetSet<TRole>().Add(role);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            this.GetSet<TRole>().Update(role);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            this.GetSet<TRole>().Remove(role);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            return Task.FromResult(this.ConvertIdentifierToString(role.ID));
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));
            ArgumentNullException.ThrowIfNullOrEmpty(roleName, nameof(roleName));

            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(role, nameof(role));
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedName, nameof(normalizedName));

            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNullOrEmpty(roleId, nameof(roleId));

            return await this.GetSet<TRole>().FindAsync(this.ConvertIdentifierFromString<TKey>(roleId));
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedRoleName, nameof(normalizedRoleName));

            return await this.GetSet<TRole>().FirstOrDefaultAsync(role => role.NormalizedName == normalizedRoleName);
        }

        #endregion
    }
}