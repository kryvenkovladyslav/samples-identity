using System;

namespace IdentitySystem.Models
{
    /// <summary>
    /// Represents a base entity for every Identity model
    /// </summary>
    /// <typeparam name="TKey">The type representing an identifier for current entity</typeparam>
    public abstract class BaseApplicationEntity<TKey> where TKey : IEquatable<TKey>   
    {
        /// <summary>
        /// The identifier for current entity
        /// </summary>
        public TKey ID { get; set; }
    }
}