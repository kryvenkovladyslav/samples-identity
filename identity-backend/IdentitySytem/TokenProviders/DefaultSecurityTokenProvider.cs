using IdentitySystem.Models;
using System;

namespace IdentitySystem.TokenProviders
{
    public class DefaultSecurityTokenProvider<TUser, TKey> : SecurityTokenProvider<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    { }
}