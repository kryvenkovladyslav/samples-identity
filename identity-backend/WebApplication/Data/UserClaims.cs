using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using WebApplication.Infrastructure.Authentication;

namespace WebApplication.Data
{
    public static class UserClaims
    {
        public static string[] Schemes { get; private set; }

        public static IEnumerable<string> Users { get; private set; }

        public static Dictionary<string, IEnumerable<ApplicationRoles>> UserClaimsData { get; private set; }

        static UserClaims()
        {
            Schemes = new string[] { AuthenticationDefaults.CustomScheme, AuthenticationDefaults.RestrictedScheme };

            UserClaimsData = new Dictionary<string, IEnumerable<ApplicationRoles>>()
            {
                { "Vladyslav", new [] { ApplicationRoles.User, ApplicationRoles.Admin } },
                { "Jack", new [] { ApplicationRoles.User, ApplicationRoles.Admin } },
                { "Tom", new [] { ApplicationRoles.User } },
                { "Liza", new [] { ApplicationRoles.User } }
            };

            Users = new List<string>
            {
                "Vladyslav", "Tom", "Jack", "Liza"
            };
        }

        public static IEnumerable<ClaimsPrincipal> GetUsers()
        {
            foreach (string scheme in Schemes)
            {
                foreach (var kvp in Claims)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, kvp.Key) }, scheme);
                    identity.AddClaims(kvp.Value);
                    yield return new ClaimsPrincipal(identity);
                }
            }
        }

        public static Dictionary<string, IEnumerable<Claim>> Claims
        {
            get => UserClaimsData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(role => new Claim(ClaimTypes.Role, Enum.GetName(role))));
        }
    }
}