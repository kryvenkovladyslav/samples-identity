using System;
using Identity.Abstract.Models;
using Identity.System.Constants;
using Identity.System.Validation;
using Identity.Abstract.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.System.Implementation;
using Identity.System.TokenProviders;
using IdentitySystem.Abstract.Interfaces;
using Identity.System.ConfirmationServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.System.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomIdentitySystem<TUser, TRole, TUserRole, TUseClaim, TKey, TContext>(this IServiceCollection services)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
            where TUser : IdentitySystemUser<TKey>
            where TRole : IdentitySystemRole<TKey>
            where TUserRole : IdentitySystemUserRole<TKey>
            where TUseClaim : IdentitySystemUserClaim<TKey>
        {
            services.TryAddSingleton<IPasswordHasher<TUser>, IdentityPasswordHasher<TUser, TKey>>();

            var userStoreType = typeof(DatabaseUserStore<,,,,,>)
                .MakeGenericType(typeof(TUser), typeof(TRole), typeof(TUserRole), typeof(TUseClaim), typeof(TKey), typeof(TContext));

            services.TryAddScoped(typeof(IUserStore<TUser>), userStoreType);

            services.AddHttpContextAccessor();
            services.TryAddScoped<ILookupNormalizer, UpperLookupNormalizer>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, PrincipalClaimsFactory<TUser, TKey>>();

            services.TryAddScoped<IPhoneNumberConfirmationService<TUser, TKey>, PhoneNumberConfirmationService<TUser, TKey>>();
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

            var phoneTokenProvider = typeof(PhoneNumberSecurityTokenProvider<,>).MakeGenericType(userType, keyType);
            var emailTokenProvider = typeof(EmailSecurityTokenProvider<,>).MakeGenericType(userType, keyType);
            var defaultTokenProvider = typeof(DefaultSecurityTokenProvider<,>).MakeGenericType(userType, keyType);

            builder.AddTokenProvider(TokenOptions.DefaultProvider, defaultTokenProvider);
            builder.AddTokenProvider(TokenOptions.DefaultPhoneProvider, phoneTokenProvider);
            builder.AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProvider);

            return builder;
        }
    }
}