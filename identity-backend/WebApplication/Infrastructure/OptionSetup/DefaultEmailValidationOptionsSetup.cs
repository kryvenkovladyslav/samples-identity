using IdentitySystem.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using WebApplication.Infrastructure.Options;

namespace WebApplication.Infrastructure.OptionSetup
{
    public class DefaultEmailValidationOptionsSetup : IConfigureOptions<EmailValidationOptions>
    {
        private readonly DefaultEmailValidationOptions defaultOption;

        public DefaultEmailValidationOptionsSetup(IOptions<DefaultEmailValidationOptions> options)
        {
            this.defaultOption = options.Value;
        }

        public void Configure(EmailValidationOptions options)
        {
            options.AllowedDomains = new List<string>(defaultOption.AllowedDomains);
        }
    }
}