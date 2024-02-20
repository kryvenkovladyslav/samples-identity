using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentitySystem.TokenProviders
{
    public class SMSSecurityTokenProvider<TUser, TKey> : SecurityTokenProvider<TUser,TKey>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
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