using System;

namespace Identity.Abstract.Models
{
    /// <summary>
    /// Represents a user for Identity System
    /// </summary>
    /// <typeparam name="TKey">The type representing an identifier for current entity</typeparam>
    public class IdentitySystemUser<TKey> : IdentitySystemEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Represents a name of a current user
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Represents a normalized name of a current user
        /// </summary>
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        /// Represents a email of a current user
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Represents a normalized email of a current user
        /// </summary>
        public virtual string NormalizedEmailAddress { get; set; }

        /// <summary>
        /// Represents a property that indicates if the user's email confirmed 
        /// </summary>
        public virtual bool IsEmailAddressConfirmed { get; set; }

        /// <summary>
        /// Represents a phone number of a current user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Represents a property that indicates if the user's phone number confirmed 
        /// </summary>
        public virtual bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// A hashed value representing a user's password
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// The default constructor is used to create an object
        /// </summary>
        public IdentitySystemUser() { }

        /// <summary>
        /// The default constructor is used to create an object using name
        /// </summary>
        /// <param name="userName">The name of a current user</param>
        public IdentitySystemUser(string userName)
        {
            this.UserName = userName;
        }

        /// <summary>
        /// Overridden method for returning a name of a current user
        /// </summary>
        /// <returns>The name of a current user</returns>
        public override string ToString()
        {
            return this.UserName ?? string.Empty;
        }
    }
}