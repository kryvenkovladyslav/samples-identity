using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Data;

namespace WebApplication.Infrastructure.Middleware
{
    public sealed class RoleMembershipMiddleware : BaseMiddleware
    {
        private readonly string authenticationType;

        public RoleMembershipMiddleware(RequestDelegate next) : base(next)
        {
            this.authenticationType = "Roles";
        }

        public override async Task Invoke(HttpContext context)
        {
            var identity = context.User.Identity;

            if(identity.IsAuthenticated && UserClaims.UserClaimsData.ContainsKey(identity.Name))
            {
                var claimsIdentity = new ClaimsIdentity(UserClaims.Claims[identity.Name], this.authenticationType);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, identity.Name));
                context.User.AddIdentity(claimsIdentity);
            }

            await this.Next(context);
        }
    }
}