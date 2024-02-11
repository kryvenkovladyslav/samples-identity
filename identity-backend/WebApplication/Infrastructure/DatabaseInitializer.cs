using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Database;
using Microsoft.EntityFrameworkCore;
using WebApplication.Infrastructure.Options;
using IdentityDataAccessLayer.Models;
using IdentityDataAccessLayer.Options;
using System.Linq;

namespace WebApplication.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IApplicationBuilder builder)
        {

            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var context = provider.GetRequiredService<DatabaseContext>();
                Initialize(provider, context);
            }
        }

        private static void Initialize(IServiceProvider serviceProvider, DatabaseContext context)
        {
            var databaseSeed = serviceProvider.GetRequiredService<IOptions<SeedUsersOptions>>().Value;

            var user = context.Users.FirstOrDefault(user => user.UserName == databaseSeed.UserName);

            if(user != null)
            {
                return;
            }

            var s = new IdentityUser
            {
                UserName = databaseSeed.UserName,
                NormalizedUserName = databaseSeed.NormalizedUserName,
                EmailAddress = databaseSeed.EmailAddress,
                NormalizedEmailAddress = databaseSeed.NormalizedEmailAddress,
                IsEmailAddressConfirmed = databaseSeed.IsEmailAddressConfirmed,
                PhoneNumber = databaseSeed.PhoneNumber,
                IsPhoneNumberConfirmed = databaseSeed.IsPhoneNumberConfirmed
            };
            context.Users.Add(s);
            context.SaveChanges();
        }
    }
}