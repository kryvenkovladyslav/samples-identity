using IdentitySystem.Abstract;
using System;
using System.Security.Claims;

namespace IdentitySystem.Models
{
    public class BaseApplicationUserClaim<TKey> 
        where TKey : IEquatable<TKey>
    {
        public virtual TKey ID { get; set; }

        public virtual TKey UserID { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }
}