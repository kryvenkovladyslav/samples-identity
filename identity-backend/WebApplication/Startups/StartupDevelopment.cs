using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Microsoft.AspNetCore.DataProtection;
using WebApplication.Infrastructure.Middleware;
using WebApplication.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApplication.Infrastructure.Authorization.Handlers;
using WebApplication.Infrastructure.Authorization;
using IdentitySystem.Extensions;
using WebApplication.Models;
using WebApplication.Infrastructure.Options;
using WebApplication.Infrastructure.OptionSetup;
using IdentitySystem.Validation;
using WebApplication.Infrastructure;
using IdentityDataAccessLayer.Models;
using WebApplication.Database;
using IdentityDataAccessLayer.Extensions;

namespace WebApplication.Startups
{
    public sealed class StartupDevelopment
    {
        public IConfiguration Configuration { get; }

        public StartupDevelopment(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DefaultEmailValidationOptions>(this.Configuration.GetSection(DefaultEmailValidationOptions.Position));
            services.ConfigureOptions<DefaultEmailValidationOptionsSetup>();

            services.Configure<ApplicationSeedUserOptions>(this.Configuration.GetSection(ApplicationSeedUserOptions.Position));
            services.ConfigureOptions<ApplicationSeedUserOptionsSetup>();

            services.Configure<DefaultConnectionStringOptions>(this.Configuration.GetSection(DefaultConnectionStringOptions.Position));
            services.ConfigureOptions<DefaultConnectionStringOptionsSetup>();

            services.AddTransient<IAuthorizationHandler, DefaultAuthorizationRequirementHandler>();

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
                .SetApplicationName("WebAPi");

            services.AddControllers();
            services.AddEndpointsApiExplorer();;
            services.AddSwaggerGen();

            services.AddDataProtection();

            services.AddIdentityDataAccess();
            services.AddDatabaseStores<IdentityUser, Guid, DatabaseContext>();

            services.AddAuthentication(options =>
            {
                options.AddScheme<ExtendedAuthenticationHandler>(AuthenticationDefaults.CustomScheme, AuthenticationDefaults.CustomScheme);
                options.DefaultScheme = AuthenticationDefaults.CustomScheme;
            });
            services.AddAuthorization(options =>
            {
                AuthorizationPolicies.AddPolicies(options);
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DatabaseInitializer.Initialize(app);

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Use this middleware to enable custom authentication middleware without authentication service being configured
            //app.UseMiddleware<CookieAuthenticationMiddleware>();

            app.UseAuthentication();

            // Use this middleware to enable middleware setting the role claim to a request

            app.UseRouting();
            //app.UseMiddleware<RoleMembershipMiddleware>();

            app.UseAuthorization();

            //app.UseMiddleware<AuthorizationReporterMiddleware>();

            app.UseMiddleware<ClaimsReporterMiddleware>();

            // Use this middleware to enable custom authorization middleware without authorization service being configured
            //app.UseMiddleware<Infrastructure.Middleware.AuthorizationMiddleware>();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}