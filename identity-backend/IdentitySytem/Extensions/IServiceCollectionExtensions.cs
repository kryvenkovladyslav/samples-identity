using IdentitySystem.Implementation;
using IdentitySystem.Models;
using IdentitySystem.Stores;
using IdentitySystem.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace IdentitySystem.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseStores<TUser, TKey, TContext>(this IServiceCollection services)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
            where TUser : BaseApplicationUser<TKey>, new()
        {
            var userIdentifierType = typeof(TUser).BaseType.GetGenericArguments()[0];

            var userStoreType = typeof(DatabaseUserStore<,,>).MakeGenericType(typeof(TUser), typeof(TKey), typeof(TContext));

            services.TryAddScoped(typeof(IUserStore<TUser>), userStoreType);
            services.AddIdentityCore<TUser>();
            return services;
        }

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