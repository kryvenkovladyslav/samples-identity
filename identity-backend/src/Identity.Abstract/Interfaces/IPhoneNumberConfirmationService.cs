using System;
using Identity.Abstract.Models;

namespace IdentitySystem.Abstract.Interfaces
{
    public interface IPhoneNumberConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public void SendMessage(TUser user, params string[] body);
    }
}