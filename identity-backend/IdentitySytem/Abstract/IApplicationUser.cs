using System;

namespace IdentitySystem.Abstract
{
    public interface IApplicationUser<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        public TIdentifier ID { get; }
        
        public string UserName { get; }

        public string NormalizedUserName { get; }
    }
}