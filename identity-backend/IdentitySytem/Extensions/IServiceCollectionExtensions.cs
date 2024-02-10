using IdentitySystem.Models;
using IdentitySystem.Stores;
using IdentitySystem.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IdentitySystem.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentitySystem<TUser>(this IServiceCollection services)
            where TUser : BaseApplicationUser, new()
        {
            services.AddEmailValidator<TUser>().AddUpperLookUpNormalizer().AddInMemoryUserStore<TUser>().AddIdentityCore<TUser>();
            return services;
        }

        public static IServiceCollection AddEmailValidator<TUser>(this IServiceCollection services)
            where TUser : BaseApplicationUser, new()
        {
            services.TryAddSingleton<IUserValidator<TUser>, EmailValidator<TUser>>();
            return services;
        }

        public static IServiceCollection AddUpperLookUpNormalizer(this IServiceCollection services)
        {
            return AddLookNormalizer<UpperInvariantLookupNormalizer>(services);
        }

        public static IServiceCollection AddLookNormalizer<TNormalizer>(this IServiceCollection services) 
            where TNormalizer : class, ILookupNormalizer
        {
            services.TryAddTransient<ILookupNormalizer, TNormalizer>();
            return services;
        }

        public static IServiceCollection AddInMemoryUserStore<TUser>(this IServiceCollection services)
            where TUser: BaseApplicationUser, new()
        {
            services.TryAddSingleton<IUserStore<TUser>, InMemoryUserStore<TUser>>();
            return services;
        }
    }
}