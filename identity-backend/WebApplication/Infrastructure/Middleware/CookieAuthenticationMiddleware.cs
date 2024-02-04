using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Infrastructure.Authentication;

namespace WebApplication.Infrastructure.Middleware
{
    public sealed class CookieAuthenticationMiddleware : BaseMiddleware
    {
        private readonly IDataProtector dataProtector;

        public CookieAuthenticationMiddleware(RequestDelegate next, IDataProtectionProvider provider) : base(next)
        {
            this.dataProtector = provider.CreateProtector(AuthenticationDefaults.CustomScheme);
        }

        public override async Task Invoke(HttpContext context)
        {
            string encodedCookie = context.Request.Cookies[AuthenticationDefaults.CustomScheme];

            if (encodedCookie != null) 
            {
                var decodedCookie = this.dataProtector.Unprotect(encodedCookie);

                if (UserClaims.Users.Contains(decodedCookie))
                {
                    var claim = new Claim(ClaimTypes.Name, decodedCookie);
                    var identity = new ClaimsIdentity(new[] { claim }, AuthenticationDefaults.CustomScheme);
                    context.User = new ClaimsPrincipal(identity);
                }
            }

            await this.Next(context);
        }
    }
}