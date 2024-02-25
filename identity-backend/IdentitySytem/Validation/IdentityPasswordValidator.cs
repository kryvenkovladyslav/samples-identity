using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentitySystem.Validation
{
    public class IdentityPasswordValidator<TUser, TKey> : PasswordValidator<TUser>, IPasswordValidator<TUser>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    {
        public override Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            /// Add you own here in order to make custom validation process
            return base.ValidateAsync(manager, user, password);
        }
    }
}