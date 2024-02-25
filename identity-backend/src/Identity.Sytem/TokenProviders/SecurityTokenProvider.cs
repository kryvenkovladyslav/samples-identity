using System;
using System.Text;
using System.Threading.Tasks;
using Identity.Abstract.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Identity.System.TokenProviders
{
    public abstract class SecurityTokenProvider<TUser, TKey> : IUserTwoFactorTokenProvider<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public virtual int CodeLength { get; private set; } = 6;

        public virtual Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(manager.SupportsUserSecurityStamp);
        }

        public virtual Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(this.GenerateSecurityCode(purpose, user));
        }

        public virtual Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            var result = this.GenerateSecurityCode(purpose, user).Equals(token);
            return Task.FromResult(result);
        }

        protected virtual string GenerateSecurityCode(string purpose, TUser user)
        {
            var hashAlgorithm = new HMACSHA1(Encoding.UTF8.GetBytes(user.SecurityStamp));

            var hashCode = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(GetData(purpose, user)));

            return BitConverter.ToString(hashCode[^CodeLength..]).Replace("-", "");
        }

        protected virtual string GetData(string purpose, TUser user)
        {
            return $"{purpose}{user.SecurityStamp}";
        } 
    }
}