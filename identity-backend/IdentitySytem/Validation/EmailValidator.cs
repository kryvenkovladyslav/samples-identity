using IdentitySystem.Models;
using IdentitySystem.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySystem.Validation
{
    public class EmailValidator<TUser, TKey> : UserValidator<TUser>, IUserValidator<TUser>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    {
        private readonly EmailValidationOptions options;

        private readonly ILookupNormalizer lookupNormalizer;
       
        public EmailValidator(IOptionsMonitor<EmailValidationOptions> monitor, ILookupNormalizer lookupNormalizer)
        {
            this.options = monitor.CurrentValue;
            this.lookupNormalizer = lookupNormalizer;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var result = await base.ValidateAsync(manager, user);
            
            if(!result.Succeeded) 
            {
                return IdentityResult.Failed(result.Errors.ToArray());
            }

            string normalizedEmail = this.lookupNormalizer.NormalizeEmail(user.EmailAddress);

            if (options.AllowedDomains.Any(domain => normalizedEmail.EndsWith($"@{domain}")))
            {
                return await Task.FromResult(IdentityResult.Success);
            }

            return IdentityResult.Failed(this.CreateValidationError());
        }

        private IdentityError CreateValidationError()
        {
            return new IdentityError
            {
                Code = "EmailValidationError",
                Description = "This domain is not allowed"
            };
        }
    }
}