using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
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
            }, new[] { AuthenticationDefaults.CustomScheme });

            options.AddPolicy(CustomUserPolicyName, new AuthorizationPolicy(new[] 
            {
                new RolesAuthorizationRequirement(new [] { Enum.GetName(ApplicationRoles.User) }),
            }, new[] { AuthenticationDefaults.CustomScheme }));

            options.AddPolicy(CustomAdministratorPolicyName, builder =>
            {
                builder.RequireRole(Enum.GetName(ApplicationRoles.Admin));
                builder.AddRequirements(new[] { new NameAuthorizationRequirement("Jack") });
                builder.AddAuthenticationSchemes(AuthenticationDefaults.CustomScheme);
            });
        }
    }
}