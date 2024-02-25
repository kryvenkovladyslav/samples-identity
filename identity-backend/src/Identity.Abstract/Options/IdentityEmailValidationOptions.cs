using System.Collections.Generic;

namespace Identity.Abstract.Options
{
    public sealed class IdentityEmailValidationOptions
    {
        public IEnumerable<string> AllowedDomains { get; set; }
    }
}