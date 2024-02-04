using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Authentication;

namespace WebApplication.Infrastructure.Middleware
{
    public sealed class QueryParameterAuthenticationMiddleware : BaseMiddleware
    {
        private readonly string requiredParameter;

        public QueryParameterAuthenticationMiddleware(RequestDelegate next) : base(next)
        {
            this.requiredParameter = "userName";
        }

        public override async Task Invoke(HttpContext context)
        {
            string userName = context.Request.Query[this.requiredParameter];

            if (userName != null) 
            {
                var claim = new Claim(ClaimTypes.Name, userName);
                var identity = new ClaimsIdentity(new[] { claim }, AuthenticationDefaults.CustomScheme);
                context.User = new ClaimsPrincipal(identity);
            }

            await this.Next(context);
        }
    }
}