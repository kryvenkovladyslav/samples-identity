using IdentitySystem.Abstract;
using System;

namespace IdentitySystem.Models
{
    public abstract class BaseApplicationUser<TIdentifier> : IApplicationUser<TIdentifier>
        where TIdentifier: IEquatable<TIdentifier>
    {
        public TIdentifier ID { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public BaseApplicationUser(string userName) 
        {
            this.ID = default(TIdentifier);
            this.UserName = userName;
            this.UserName = userName.ToUpper();
        }
    }
}