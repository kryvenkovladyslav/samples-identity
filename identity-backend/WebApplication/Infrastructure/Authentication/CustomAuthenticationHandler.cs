using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Data;

namespace WebApplication.Infrastructure.Authentication
{
    public sealed class CustomAuthenticationHandler : IAuthenticationHandler
    {
        private HttpContext context;

        private AuthenticationScheme scheme;

        private readonly IDataProtector dataProtector;

        public CustomAuthenticationHandler(IDataProtectionProvider provider)
        {
            this.dataProtector = provider.CreateProtector(AuthenticationDefaults.CustomScheme);
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            string encodedCookie = this.context.Request.Cookies[AuthenticationDefaults.CustomScheme];

            if (encodedCookie == null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var decodedCookie = this.dataProtector.Unprotect(encodedCookie);

            if (!UserClaims.Users.Contains(decodedCookie))
            {
                return Task.FromResult(AuthenticateResult.Fail("Something wrong with credentials"));
            }

            var claim = new Claim(ClaimTypes.Name, decodedCookie);
            var identity = new ClaimsIdentity(new[] { claim }, AuthenticationDefaults.CustomScheme);

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), this.scheme.Name)));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            this.context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            this.context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this.scheme = scheme;
            this.context = context;
            await Task.CompletedTask;
        }
    }
}