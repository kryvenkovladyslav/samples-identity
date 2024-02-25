using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Identity.Abstract.Options;
using WebApplication.Infrastructure.Options;

namespace WebApplication.Infrastructure.OptionSetup
{
    public sealed class DefaultEmailValidationOptionsSetup : IConfigureOptions<IdentityEmailValidationOptions>
    {
        private readonly DefaultEmailValidationOptions defaultOption;

        public DefaultEmailValidationOptionsSetup(IOptions<DefaultEmailValidationOptions> options)
        {
            this.defaultOption = options.Value;
        }

        public void Configure(IdentityEmailValidationOptions options)
        {
            options.AllowedDomains = new List<string>(defaultOption.AllowedDomains);
        }
    }
}