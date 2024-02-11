using Microsoft.Extensions.DependencyInjection;
using WebApplication.Database;

namespace IdentityDataAccessLayer.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityDataAccess(this IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>();
            return services;
        }
    }
}