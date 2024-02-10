using System.Collections.Generic;

namespace IdentitySystem.Options
{
    public sealed class EmailValidationOptions
    {
        public IEnumerable<string> AllowedDomains { get; set; }
    }
}