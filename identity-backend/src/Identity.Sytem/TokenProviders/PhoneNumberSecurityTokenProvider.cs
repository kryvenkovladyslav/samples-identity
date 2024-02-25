using System;
using System.Threading.Tasks;
using Identity.Abstract.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.System.TokenProviders
{
    public class PhoneNumberSecurityTokenProvider<TUser, TKey> : SecurityTokenProvider<TUser,TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public async override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            var result = await base.CanGenerateTwoFactorTokenAsync(manager, user) &&
                !string.IsNullOrEmpty(user.EmailAddress) &&
                !user.IsEmailAddressConfirmed;

            return await Task.FromResult(result);
        }
    }
}