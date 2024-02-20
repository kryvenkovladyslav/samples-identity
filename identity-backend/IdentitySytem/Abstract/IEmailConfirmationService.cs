using IdentitySystem.Models;
using System;

namespace IdentitySystem.Abstract
{
    public interface IEmailConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    {
        public void SendMessage(TUser user, string subject, params string[] body);
    }
}