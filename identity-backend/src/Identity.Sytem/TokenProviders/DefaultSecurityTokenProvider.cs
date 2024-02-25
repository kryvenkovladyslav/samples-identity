using System;
using Identity.Abstract.Models;

namespace Identity.System.TokenProviders
{
    public class DefaultSecurityTokenProvider<TUser, TKey> : SecurityTokenProvider<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    { }
}