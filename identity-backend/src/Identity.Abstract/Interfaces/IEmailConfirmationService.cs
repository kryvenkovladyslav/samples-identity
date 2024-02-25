using System;
using Identity.Abstract.Models;

namespace Identity.Abstract.Interfaces
{
    public interface IEmailConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public void SendMessage(TUser user, string subject, params string[] body);
    }
}