using IdentityDataAccessLayer.Options;
using Microsoft.Extensions.Options;
using WebApplication.Infrastructure.Options;

namespace WebApplication.Infrastructure.OptionSetup
{
    public class ApplicationSeedUserOptionsSetup : IConfigureOptions<SeedUsersOptions>
    {
        private readonly ApplicationSeedUserOptions options;

        public ApplicationSeedUserOptionsSetup(IOptionsMonitor<ApplicationSeedUserOptions> options) 
        {
            this.options = options.CurrentValue;
        }

        public void Configure(SeedUsersOptions options)
        {
            options.UserName = this.options.UserName;
            options.NormalizedUserName = this.options.NormalizedUserName;
            options.EmailAddress = this.options.EmailAddress;
            options.NormalizedEmailAddress = this.options.NormalizedEmailAddress;
            options.IsEmailAddressConfirmed = this.options.IsEmailAddressConfirmed;
            options.PhoneNumber = this.options.PhoneNumber;
            options.IsPhoneNumberConfirmed = this.options.IsPhoneNumberConfirmed;
        }
    }
}