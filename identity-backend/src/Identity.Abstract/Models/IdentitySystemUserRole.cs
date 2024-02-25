using System;

namespace Identity.Abstract.Models
{
    /// <summary>
    /// Represents a current role for Identity user
    /// </summary>
    /// <typeparam name="TKey">The type representing an identifier for current entity</typeparam>
    public class IdentitySystemUserRole<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Represents a user identifier
        /// </summary>
        public virtual TKey UserID { get; set; }

        /// <summary>
        /// Represents a role identifier
        /// </summary>
        public virtual TKey RoleID { get; set; }
    }
}