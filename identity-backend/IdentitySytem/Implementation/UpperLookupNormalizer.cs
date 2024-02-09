using Microsoft.AspNetCore.Identity;
using System;
using System.Diagnostics.CodeAnalysis;

namespace IdentitySystem.Implementation
{
    public class UpperLookupNormalizer : ILookupNormalizer
    {
        [return: NotNullIfNotNull("email")]
        public string NormalizeEmail(string email)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(email);
            return email.Normalize().ToUpperInvariant();
        }

        [return: NotNullIfNotNull("name")]
        public string NormalizeName(string name)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(name);
            return name.Normalize().ToUpperInvariant();
        }
    }
}