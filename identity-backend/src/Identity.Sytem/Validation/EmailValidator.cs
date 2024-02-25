using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.Abstract.Models;
using Identity.Abstract.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Identity.System.Validation
{
    public class EmailValidator<TUser, TKey> : UserValidator<TUser>, IUserValidator<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        private readonly IdentityEmailValidationOptions options;

        private readonly ILookupNormalizer lookupNormalizer;
       
        public EmailValidator(IOptionsMonitor<IdentityEmailValidationOptions> monitor, ILookupNormalizer lookupNormalizer)
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