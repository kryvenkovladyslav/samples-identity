using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Abstract.Models;
using Identity.System.Constants;
using Microsoft.AspNetCore.Identity;

namespace Identity.System.Implementation
{
    public class PrincipalClaimsFactory<TUser, TKey> : IUserClaimsPrincipalFactory<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        private readonly UserManager<TUser> userManager;

        public PrincipalClaimsFactory(UserManager<TUser> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public virtual async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            var identity = await this.GenerateIdentityAsync(user).ConfigureAwait(false);
            return new ClaimsPrincipal(identity);
        }

        protected virtual async Task<ClaimsIdentity> GenerateIdentityAsync(TUser user)
        {
            var userID = await this.userManager.GetUserIdAsync(user).ConfigureAwait(false);
            var userName = await this.userManager.GetUserNameAsync(user).ConfigureAwait(false);
            var identity = new ClaimsIdentity(IdentityAuthenticationDefaults.ApplicationScheme);
            identity.AddClaims(new[] 
            { 
                new Claim(ClaimTypes.Name, userName), 
                new Claim(ClaimTypes.NameIdentifier, userID) 
            });

            if (this.userManager.SupportsUserEmail)
            {
                var userEmail = await this.userManager.GetEmailAsync(user).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(userEmail))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, userEmail));
                }
            }
            if (this.userManager.SupportsUserClaim)
            {
                var userClaims= await this.userManager.GetClaimsAsync(user).ConfigureAwait(false);
                identity.AddClaims(userClaims);
            }

            return identity;
        }
    }
}