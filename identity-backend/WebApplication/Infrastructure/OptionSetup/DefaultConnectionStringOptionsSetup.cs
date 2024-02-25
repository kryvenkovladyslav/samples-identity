using Microsoft.Extensions.Options;
using IdentityDataAccessLayer.Options;
using WebApplication.Infrastructure.Options;

namespace WebApplication.Infrastructure.OptionSetup
{
    public sealed class DefaultConnectionStringOptionsSetup : IConfigureOptions<ConnectionStringOptions>
    {
        private readonly DefaultConnectionStringOptions options;

        public DefaultConnectionStringOptionsSetup(IOptionsMonitor<DefaultConnectionStringOptions> options)
        {
            this.options = options.CurrentValue;
        }

        public void Configure(ConnectionStringOptions options)
        {
            options.ConnectionString = this.options.IdentityDatabase;
        }
    }
}