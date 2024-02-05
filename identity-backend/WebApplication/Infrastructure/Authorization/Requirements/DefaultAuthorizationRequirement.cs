using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Infrastructure.Authorization.Requirements
{
    public sealed class DefaultAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Name { get; set; }
    }
}