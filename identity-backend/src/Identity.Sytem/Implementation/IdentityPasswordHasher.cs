using System;
using System.Text;
using Identity.Abstract.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Identity.System.Implementation
{
    public class IdentityPasswordHasher<TUser, TKey> : IPasswordHasher<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public IdentityPasswordHasher() { }

        public virtual string HashPassword(TUser user, string password)
        {
            var hashAlgorithm = new HMACSHA256(Encoding.UTF8.GetBytes(user.UserName));
            return BitConverter.ToString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public virtual PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            return this.HashPassword(user, providedPassword) == (hashedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}