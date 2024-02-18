using System;

namespace IdentitySystem.Models
{
    /// <summary>
    /// Represents an application Identity Role entity
    /// </summary>
    /// <typeparam name="TKey">The type representing an identifier for current entity</typeparam>
    public class BaseApplicationRole<TKey> : BaseApplicationEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The name of current role
        /// </summary>
        public virtual string Name { get; set; }    

        /// <summary>
        /// The normalized name of current entity
        /// </summary>
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// The ConcurrencyStamp of current entity
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; }

        /// <summary>
        /// The default constructor is used to create an Identity Role
        /// </summary>
        public BaseApplicationRole() { }

        /// <summary>
        /// The constructor is used to create an Identity role with a name
        /// </summary>
        /// <param name="roleName">The name of current role</param>
        /// <exception cref="ArgumentNullException">throws is a name of the role is null</exception>
        public BaseApplicationRole(string roleName) 
        {
            this.Name = roleName ?? throw new ArgumentNullException(nameof(roleName));
        }

        /// <summary>
        /// Overridden method for returning a name of current role
        /// </summary>
        /// <returns>The name of current role</returns>
        public override string ToString()
        {
            return this.Name ?? string.Empty;
        }
    }
}