using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace IdentitySystem.Implementation
{
    public class DatabaseUserStore<TUser, TRole, TUserRole, TUserClaim, TKey, TContext> : DatabaseStore<TContext>,
        IUserStore<TUser>, 
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPasswordStore<TUser>,
        IQueryableUserStore<TUser>, 
        IUserPhoneNumberStore<TUser>,
        IUserSecurityStampStore<TUser>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>, new()
        where TRole : BaseApplicationRole<TKey>, new()
        where TUserRole : BaseApplicationUserRole<TKey>, new()
        where TUserClaim: BaseApplicationUserClaim<TKey>, new()
    {
        private readonly IPasswordHasher<TUser> passwordHasher;

        public IQueryable<TUser> Users
        {
            get
            {
                return this.GetSet<TUser>();
            }
        }

        public DatabaseUserStore(TContext context, IPasswordHasher<TUser> passwordHasher) : base(context) 
        {
            this.passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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

            var id = this.ConvertIdentifierFromString<TKey>(userId);
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

            foreach (var currentClaim in claims)
            {
                var requiredClaims = await this.GetSet<TUserClaim>()
                    .FirstOrDefaultAsync(userClaim => userClaim.UserID.Equals(user.ID) && 
                    userClaim.ClaimValue == currentClaim.Value && userClaim.ClaimType == currentClaim.Type);

                this.GetSet<TUserClaim>().Remove(requiredClaims);
            }
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

        #region IRoleStore Implementation

        public virtual async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(roleName, nameof(roleName));

            var role = await this.GetSet<TRole>().SingleOrDefaultAsync(role => role.Name.ToUpper() == roleName.ToUpper(), cancellationToken);

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var userRole = new TUserRole
            {
                UserID = user.ID,
                RoleID = role.ID
            };

            this.GetSet<TUserClaim>().Add(new TUserClaim
            {
                UserID = user.ID,
                ClaimType = ClaimTypes.Role,
                ClaimValue = roleName
            });
            this.GetSet<TUserRole>().Add(userRole);
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(roleName, nameof(roleName));

            var role = await this.GetSet<TRole>().SingleOrDefaultAsync(role => role.Name.ToUpper() == roleName.ToUpper(), cancellationToken);

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var userRole = await this.GetSet<TUserRole>()
                .SingleOrDefaultAsync(userRole => userRole.RoleID.Equals(role.ID) && userRole.UserID.Equals(user.ID), cancellationToken);

            this.GetSet<TUserRole>().Remove(userRole);
        }

        public virtual async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var roleIdentifiers = await this.GetSet<TUserRole>()
                .Where(userRole => userRole.UserID.Equals(user.ID))
                .Select(userRole => userRole.RoleID)
                .ToListAsync(cancellationToken);

            var roles = await this.GetSet<TRole>()
                .Where(role => roleIdentifiers.Contains(role.ID))
                .Select(role => role.Name)
                .ToListAsync(cancellationToken);

            return roles;
        }

        public virtual async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var role = await this.GetSet<TRole>().FirstOrDefaultAsync(role => role.Name.ToUpper() == roleName.ToUpper(), cancellationToken);

            var users = await this.GetSet<TUserRole>()
                .Where(userRole => userRole.RoleID.Equals(role.ID) && userRole.UserID.Equals(user.ID))
                .ToListAsync(cancellationToken);

            return await Task.FromResult(users.Count > 0);
        }

        public virtual async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNullOrEmpty(roleName, nameof(roleName));

            var role = await this.GetSet<TRole>().FirstOrDefaultAsync(role => role.Name.ToUpper() == roleName.ToUpper(), cancellationToken);

            var userRoles = await this.GetSet<TUserRole>()
                .Where(userRole => userRole.RoleID.Equals(role.ID))
                .Select(userRole => userRole.UserID)
                .ToListAsync(cancellationToken);

            var users = await this.GetSet<TUser>().Where(user => userRoles.Contains(user.ID)).ToListAsync(cancellationToken);

            return users;
        }

        #endregion

        #region IUserSecurityStampStore Implementation

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(stamp, nameof(stamp));

            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region IUserPasswordStore Implementation

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNullOrEmpty(passwordHash, nameof(passwordHash));

            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        #endregion
    }
}