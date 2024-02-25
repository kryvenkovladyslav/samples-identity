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
using IdentitySystem.Constants;

namespace IdentitySystem.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomIdentitySystem<TUser, TRole, TUserRole, TUseClaim, TKey, TContext>(this IServiceCollection services)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
            where TUser : BaseApplicationUser<TKey>
            where TRole : BaseApplicationRole<TKey>
            where TUserRole : BaseApplicationUserRole<TKey>
            where TUseClaim : BaseApplicationUserClaim<TKey>
        {
            services.TryAddSingleton<IPasswordHasher<TUser>, IdentityPasswordHasher<TUser, TKey>>();

            var userStoreType = typeof(DatabaseUserStore<,,,,,>)
                .MakeGenericType(typeof(TUser), typeof(TRole), typeof(TUserRole), typeof(TUseClaim), typeof(TKey), typeof(TContext));

            services.TryAddScoped(typeof(IUserStore<TUser>), userStoreType);

            services.AddHttpContextAccessor();
            services.TryAddScoped<ILookupNormalizer, UpperLookupNormalizer>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, PrincipalClaimsFactory<TUser, TKey>>();

            services.TryAddScoped<ISMSConfirmationService<TUser, TKey>, SMSConfirmationService<TUser, TKey>>();
            services.TryAddScoped<IEmailConfirmationService<TUser, TKey>, EmailConfirmationService<TUser, TKey>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityAuthenticationDefaults.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityAuthenticationDefaults.ApplicationScheme;
                options.DefaultSignInScheme = IdentityAuthenticationDefaults.ExternalScheme;
            })
                .AddCookie(IdentityAuthenticationDefaults.ApplicationScheme);

            services.AddIdentityCore<TUser>(options =>
            {
                options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
            }).AddTokenDefaultProviders();

            services.TryAddSingleton<IUserValidator<TUser>, EmailValidator<TUser, TKey>>();
            services.TryAddSingleton<IPasswordValidator<TUser>, IdentityPasswordValidator<TUser, TKey>>();


            return services;
        }

        private static IdentityBuilder AddTokenDefaultProviders(this IdentityBuilder builder)
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

        public static IServiceCollection AddInMemoryUserStore<TUser>(this IServiceCollection services)
            where TUser: BaseApplicationUser, new()
        {
            services.TryAddSingleton<IUserStore<TUser>, InMemoryUserStore<TUser>>();
            return services;
        }
    }
}