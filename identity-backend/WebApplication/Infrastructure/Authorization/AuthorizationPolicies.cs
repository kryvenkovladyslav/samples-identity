using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using WebApplication.Data;
using WebApplication.Infrastructure.Authentication;
using WebApplication.Infrastructure.Authorization.Requirements;

namespace WebApplication.Infrastructure.Authorization
{
    public static class AuthorizationPolicies
    {
        public static string CustomAdministratorPolicyName { get; private set; } = nameof(CustomAdministratorPolicyName);
        public static string CustomUserPolicyName { get; private set; } = nameof(CustomUserPolicyName);

        public static void AddPolicies(AuthorizationOptions options)
        {
            options.DefaultPolicy = new AuthorizationPolicy(new IAuthorizationRequirement[] 
            { 
                new DefaultAuthorizationRequirement { Name = "Vladyslav" },
                new RolesAuthorizationRequirement(new [] {  Enum.GetName(ApplicationRoles.Tester) }),
            }, new[] { IdentityConstants.ApplicationScheme });

            options.AddPolicy(CustomUserPolicyName, new AuthorizationPolicy(new[] 
            {
                new RolesAuthorizationRequirement(new [] { Enum.GetName(ApplicationRoles.User) }),
            }, new[] { AuthenticationDefaults.CustomScheme }));

            options.AddPolicy(CustomAdministratorPolicyName, builder =>
            {
                builder.RequireClaim(ClaimTypes.Role, new[] { Enum.GetName(ApplicationRoles.Admin).ToUpper() });
                builder.AddRequirements(new[] { new NameAuthorizationRequirement("rita") });
                builder.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
            });
        }
    }
}