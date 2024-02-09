using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Infrastructure.Authentication;

namespace WebApplication.Infrastructure.Middleware
{
    public class AuthorizationReporterMiddleware : BaseMiddleware
    {
        private string[] schemes = new string[] { AuthenticationDefaults.CustomScheme };
        private IAuthorizationPolicyProvider policyProvider;
        private IAuthorizationService authorizationService;

        public AuthorizationReporterMiddleware(RequestDelegate requestDelegate,IAuthorizationPolicyProvider provider, IAuthorizationService service) 
            : base(requestDelegate)
        {
            this.policyProvider = provider;
            this.authorizationService = service;
        }

        public override async Task Invoke(HttpContext context)
        {
            Endpoint endpoint  = context.GetEndpoint();

            if (endpoint != null)
            {
                Dictionary<(string, string), bool> results = new Dictionary<(string, string), bool>();

                bool isAllowAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;

                IEnumerable<IAuthorizeData> authorizeData = endpoint?.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();

                AuthorizationPolicy policy = await AuthorizationPolicy.CombineAsync(this.policyProvider, authorizeData);

                foreach (ClaimsPrincipal cp in this.GetUsers())
                {
                    results[(cp.Identity.Name, cp.Identity.AuthenticationType)] = isAllowAnonymous || policy == null || await this.AuthorizeUser(cp, policy);
                }

                context.Items["authReport"] = results;
                await endpoint.RequestDelegate(context);
            }
            else
            {
                await this.Next(context);
            }
        }

        private IEnumerable<ClaimsPrincipal> GetUsers()
        {
            return UserClaims.GetUsers().Concat(new[] { new ClaimsPrincipal(new ClaimsIdentity()) });
        }

        private async Task<bool> AuthorizeUser(ClaimsPrincipal principal, AuthorizationPolicy policy)
        {
            var matchingResult = this.UserSchemeMatchesPolicySchemes(principal, policy);
            var authorizationResult = (await this.authorizationService.AuthorizeAsync(principal, policy)).Succeeded;
            return matchingResult && authorizationResult;
        }

        private bool UserSchemeMatchesPolicySchemes(ClaimsPrincipal principal, AuthorizationPolicy policy)
        {
            return policy.AuthenticationSchemes?.Count() == 0 || principal.Identities.Select(id => id.AuthenticationType)
                .Any(auth => policy.AuthenticationSchemes.Any(scheme => scheme == auth));
        }
    }
}
