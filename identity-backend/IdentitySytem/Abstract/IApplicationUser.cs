using System;

namespace IdentitySystem.Abstract
{
    public interface IApplicationUser<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        public TIdentifier ID { get; set; }
        
        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }
    }
}