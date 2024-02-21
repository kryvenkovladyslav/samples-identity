using IdentitySystem.Abstract;
using IdentitySystem.ConfirmationServices;
using IdentitySystem.TokenProviders;
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
        public static IServiceCollection AddEntityFrameworkStores<TUser, TRole, TUserRole, TUseClaim, TKey, TContext>(this IServiceCollection services)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
            where TUser : BaseApplicationUser<TKey>
            where TRole : BaseApplicationRole<TKey>
            where TUserRole : BaseApplicationUserRole<TKey>
            where TUseClaim: BaseApplicationUserClaim<TKey>
        {
            var userIdentifierType = typeof(TUser).BaseType.GetGenericArguments()[0];

            var userStoreType = typeof(DatabaseUserStore<,,,,,>).MakeGenericType(typeof(TUser), typeof(TRole), typeof(TUserRole), typeof(TUseClaim), typeof(TKey), typeof(TContext));

            services.TryAddScoped(typeof(IUserStore<TUser>), userStoreType);
            services.AddIdentityCore<TUser>(options =>
            {
                options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
                .AddTokenProvider(TokenOptions.DefaultPhoneProvider, typeof(SMSSecurityTokenProvider<TUser, TKey>))
                .AddTokenProvider(TokenOptions.DefaultEmailProvider, typeof(EmailSecurityTokenProvider<TUser, TKey>));

            return services;
        }

        public static IdentityBuilder AddTokenDefaultProviders(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var keyType = userType.BaseType.GetGenericArguments()[0];

            var phoneTokenProvider = typeof(SMSSecurityTokenProvider<,>).MakeGenericType(userType, keyType);
            var emailTokenProvider = typeof(EmailSecurityTokenProvider<,>).MakeGenericType(userType, keyType);
            var defaultTokenProvider = typeof(DefaultSecurityTokenProvider<,>).MakeGenericType(userType, keyType);

            

            builder.AddTokenProvider(TokenOptions.DefaultProvider, defaultTokenProvider);
            builder.AddTokenProvider(TokenOptions.DefaultPhoneProvider, phoneTokenProvider);
            builder.AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProvider);

            return builder;
        }

        public static IServiceCollection AddConfirmationServices<TUser, TKey>(this IServiceCollection services)
            where TKey : IEquatable<TKey>
            where TUser : BaseApplicationUser<TKey>
        {
            services.TryAddScoped<ISMSConfirmationService<TUser, TKey>, SMSConfirmationService<TUser, TKey>>();
            services.TryAddScoped<IEmailConfirmationService<TUser, TKey>, EmailConfirmationService<TUser, TKey>>();

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