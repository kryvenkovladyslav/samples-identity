using System;
using System.Threading.Tasks;
using Identity.Abstract.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.System.Validation
{
    public class IdentityPasswordValidator<TUser, TKey> : PasswordValidator<TUser>, IPasswordValidator<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public override Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            /// Add you own here in order to make custom validation process
            return base.ValidateAsync(manager, user, password);
        }
    }
}