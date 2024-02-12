using System;
using System.Security.Claims;

namespace IdentitySystem.Abstract
{
    public interface IApplicationUserClaim<TKey> where TKey: IEquatable<TKey>
    {
        public TKey ID { get; set; }

        public TKey UserID { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}