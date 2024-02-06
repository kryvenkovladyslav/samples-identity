using IdentitySystem.Abstract;
using System;

namespace IdentitySystem.Models
{
    public abstract class BaseApplicationUser<TIdentifier> : IApplicationUser<TIdentifier>
        where TIdentifier: IEquatable<TIdentifier>
    {
        public TIdentifier ID { get; private set; }

        public string UserName { get; private set; }

        public string NormalizedUserName { get; private set; }

        public BaseApplicationUser(string userName) 
        {
            this.ID = default(TIdentifier);
            this.UserName = userName;
            this.UserName = userName.ToUpper();
        }
    }
}