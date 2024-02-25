using System;

namespace Identity.Abstract.Models
{
    /// <summary>
    /// Represents a current claim for Identity user
    /// </summary>
    /// <typeparam name="TKey">The type representing an identifier for current entity</typeparam>
    public class IdentitySystemUserClaim<TKey> : IdentitySystemEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Represents a user identifier
        /// </summary>
        public virtual TKey UserID { get; set; }

        /// <summary>
        /// Represents a type of a current claim 
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Represents a value of current claim type
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}