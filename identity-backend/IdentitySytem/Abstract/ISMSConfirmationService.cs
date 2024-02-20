using IdentitySystem.Models;
using System;

namespace IdentitySystem.Abstract
{
    public interface ISMSConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    {
        public void SendMessage(TUser user, params string[] body);
    }
}