using IdentitySystem.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentitySystem.Implementation
{
    public class IdentityPasswordHasher<TUser, TKey> : IPasswordHasher<TUser>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
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