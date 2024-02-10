using IdentitySystem.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentitySystem.Stores
{
    public class InMemoryUserStore<TUser> : 
        IUserStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IQueryableUserStore<TUser>
        where TUser : class, IApplicationUser<string>, new()

    {
        private readonly ILookupNormalizer lookupNormalizer;

        private readonly ConcurrentDictionary<string, TUser> users;

        public InMemoryUserStore(ILookupNormalizer lookupNormalizer)
        {
            this.lookupNormalizer = lookupNormalizer;
            this.users = new ConcurrentDictionary<string, TUser>();

            var name = "Ashley";
            var email = "ashley.example@gmail.com";
            var id = Guid.NewGuid().ToString();
            this.users.TryAdd(id, new TUser
            {
                ID = id,
                UserName = name,
                NormalizedUserName = this.lookupNormalizer.NormalizeName(name),
                EmailAddress = email,
                NormalizedEmailAddress = this.lookupNormalizer.NormalizeEmail(email),
                PhoneNumber = "123-456",
                IsPhoneNumberConfirmed = true,
                IsEmailAddressConfirmed = true
            });
        }

        public IQueryable<TUser> Users
        {
            get => this.users.Select(kvp => kvp.Value).AsQueryable();
        }

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            if (!this.users.ContainsKey(user.ID) && this.users.TryAdd(user.ID, user))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(this.CreateIdentityError());
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
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

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedEmail);

            return Task.FromResult(this.users.Values.FirstOrDefault(user => user.NormalizedEmailAddress == normalizedEmail));
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(userId);

            return Task.FromResult(this.users[userId]);
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNullOrEmpty(normalizedUserName);

            return Task.FromResult(this.users.Values.FirstOrDefault(user => user.NormalizedUserName == normalizedUserName));
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.IsEmailAddressConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedEmailAddress);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.IsPhoneNumberConfirmed);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.ID);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.UserName);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(email);
            user.EmailAddress = email;

            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            user.IsEmailAddressConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(normalizedEmail);
            user.NormalizedEmailAddress = normalizedEmail;

            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.NormalizedUserName = normalizedName);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(phoneNumber);
            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);
            user.IsPhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            return Task.FromResult(user.UserName = userName);
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(user);

            if (this.users.ContainsKey(user.ID))
            {
                var currentUser = this.users[user.ID];
                currentUser.UserName = user.UserName;
                currentUser.NormalizedUserName = user.NormalizedUserName;

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