using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication.Infrastructure.Middleware
{
    public sealed class AuthorizationMiddleware : BaseMiddleware
    {
        private readonly string requiredRole;
        
        public AuthorizationMiddleware(RequestDelegate next) : base(next)
        {
            this.requiredRole = "Admin";
        }

        public override async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await this.SendChallengeResponse(context);
            }

            if (!context.User.IsInRole(this.requiredRole))
            {
                await this.SendForbiddenResponse(context);
            }

            await this.Next(context);
        }

        private async Task SendChallengeResponse(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await this.Next(context);
        }

        private async Task SendForbiddenResponse(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await this.Next(context);
        }
    }
}