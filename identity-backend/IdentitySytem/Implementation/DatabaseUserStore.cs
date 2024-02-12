using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IdentitySystem.Implementation
{
    public class DatabaseUserStore<TUser,TUserClaim, TKey, TContext> : 
        IUserStore<TUser>, 
        IUserClaimStore<TUser>,
        IQueryableUserStore<TUser>, 
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>
        where TKey : IEquatable<TKey>
        where TUserClaim: BaseApplicationUserClaim<TKey>, new()
        where TContext: DbContext
        where TUser: BaseApplicationUser<TKey>, new()

    {
        private bool disposed;

        private readonly TContext context;

        public IQueryable<TUser> Users
        {
            get
            {
                return this.GetSet<TUser>();
            }
        }

        public DatabaseUserStore(TContext context) 
        {
            this.disposed = false;
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region IUserStore Implementation

        public virtual async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            this.GetSet<TUser>().Add(user);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async virtual Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            
            this.GetSet<TUser>().Remove(user);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public virtual Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();

            var id = this.ConvertIdentifierFromString(userId);
            return this.GetSet<TUser>().FindAsync(new object[] { id }, cancellationToken).AsTask();
        }

        public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedUserName, nameof(normalizedUserName));

            return this.GetSet<TUser>().FirstOrDefaultAsync(user => user.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(this.ConvertIdentifierToString(user.ID));
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedName, nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(userName, nameof(userName));

            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            this.GetSet<TUser>().Update(user);
            await this.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        #endregion

        #region IUserEmailStore and IUserPhoneStore Implementation

        public virtual Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.IsPhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            user.IsPhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public virtual Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(email , nameof(email));

            user.EmailAddress = email;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.EmailAddress);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.IsEmailAddressConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            user.IsEmailAddressConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async virtual Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedEmail, nameof(normalizedEmail));

            var user = await this.GetSet<TUser>().FirstOrDefaultAsync(user => user.NormalizedEmailAddress == normalizedEmail, cancellationToken);
            return user;
        }

        public virtual Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public virtual Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedEmail, nameof(normalizedEmail));

            user.NormalizedEmailAddress = normalizedEmail;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserClaimStore Implementation

        public virtual async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var claims = await this.GetSet<TUserClaim>()
                .Where(claim => claim.UserID.Equals(user.ID))
                .Select(claim => new Claim(claim.ClaimType, claim.ClaimValue))
                .ToListAsync(cancellationToken);

            return claims;
        }

        public virtual Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));

            foreach (var claim in claims)
            {
                this.GetSet<TUserClaim>().Add(new TUserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                    UserID = user.ID,
                });
            }

            return Task.CompletedTask;
        }

        public async virtual Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(claim, nameof(claim));
            ArgumentNullException.ThrowIfNull(newClaim, nameof(newClaim));

            var matchedClaims = await this.GetSet<TUserClaim>()
                .Where(userClaim => userClaim.UserID.Equals(user.ID) && userClaim.ClaimValue == claim.Value && userClaim.ClaimType == claim.Type)
                .ToListAsync(cancellationToken);

            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async virtual Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(claims, nameof(claims));

            var requiredClaims = await this.GetSet<TUserClaim>()
                .Where(userClaim => claims.Any(claim => claim.Value == userClaim.ClaimValue && claim.Type == userClaim.ClaimType) && userClaim.UserID.Equals(user.ID))
                .ToListAsync(cancellationToken);

            this.GetSet<TUserClaim>().RemoveRange(requiredClaims);
        }

        public async virtual Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(claim, nameof(claim));

            var identifiers = await this.GetSet<TUserClaim>()
                .Where(userClaim => userClaim.ClaimValue == claim.Value && userClaim.ClaimType == claim.ValueType)
                .Select(userClaim => userClaim.UserID)
                .ToListAsync(cancellationToken);

            var users = await this.GetSet<TUser>().Where(user => identifiers.Any(id => user.ID.Equals(id))).ToListAsync(cancellationToken);
            return users;
        }

        #endregion

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

        private DbSet<TEntity> GetSet<TEntity>() where TEntity: class
        {
            return this.context.Set<TEntity>();
        }
    }
}