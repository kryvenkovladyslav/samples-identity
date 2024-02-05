using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Authorization.Requirements;

namespace WebApplication.Infrastructure.Authorization.Handlers
{
    public sealed class DefaultAuthorizationRequirementHandler : AuthorizationHandler<DefaultAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultAuthorizationRequirement requirement)
        {
            if (context.User.Identities.Any(identity => string.Equals(identity.Name, requirement.Name, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}