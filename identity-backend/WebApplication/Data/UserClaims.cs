using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApplication.Data
{
    public static class UserClaims
    {
        public static Dictionary<string, IEnumerable<string>> UserClaimsData { get; private set; }

        public static IEnumerable<string> Users { get; private set; }

        static UserClaims()
        {
            UserClaimsData = new Dictionary<string, IEnumerable<string>>()
            {
                { "Vladyslav", new [] { "User", "Admin" } },
                { "Jack", new [] { "User", "Admin" } },
                { "Tom", new [] { "User" } },
                { "Liza", new [] { "User" } }
            };

            Users = new List<string>
            {
                "Vladyslav", "Tom", "Jack", "Liza"
            };
        }

        public static IEnumerable<string> GetUsers()
        {
            return UserClaimsData.Keys;
        }

        public static Dictionary<string, IEnumerable<Claim>> Claims =>
            UserClaimsData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(role => new Claim(ClaimTypes.Role, role)));

    }
}