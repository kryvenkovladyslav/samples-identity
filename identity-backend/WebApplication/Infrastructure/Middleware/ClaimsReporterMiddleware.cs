using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WebApplication.Infrastructure.Middleware
{
    public class ClaimsReporterMiddleware : BaseMiddleware
    {
        private readonly ILogger logger;

        public ClaimsReporterMiddleware(RequestDelegate next, ILogger<ClaimsReporterMiddleware> logger) : base(next) 
        {
            this.logger = logger;
        }

        public async override Task InvokeAsync(HttpContext context)
        {
            ClaimsPrincipal principal = context.User;

            this.logger.LogInformation($"User: { principal.Identity.Name }");
            this.logger.LogInformation($"Authenticated: { principal.Identity.IsAuthenticated }");
            this.logger.LogInformation($"Authentication Type: { principal.Identity.AuthenticationType }");

            this.logger.LogInformation($"Identities: { principal.Identities.Count() }");
            foreach (var identity in principal.Identities)
            {
                this.logger.LogInformation($"Authentication type: { identity.AuthenticationType}, { identity.Claims.Count() } claims");
                foreach (var claim in identity.Claims)
                {
                    Console.WriteLine($"Type: { GetName(claim.Type) }, Value: { claim.Value }, Issuer: { claim.Issuer }");
                }
            }
            
            await this.Next(context);
        }

        private string GetName(string claimType)
        {
            return Path.GetFileName(new Uri(claimType).LocalPath);
        }
    }
}