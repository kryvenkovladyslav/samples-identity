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
            services.AddTransient<IAuthorizationHandler, DefaultAuthorizationRequirementHandler>();

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
                .SetApplicationName("WebAPi");

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDataProtection();

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
            //app.UseMiddleware<RoleMembershipMiddleware>();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseMiddleware<AuthorizationReporterMiddleware>();

            //app.UseMiddleware<ClaimsReporterMiddleware>();

            // Use this middleware to enable custom authorization middleware without authorization service being configured
            //app.UseMiddleware<Infrastructure.Middleware.AuthorizationMiddleware>();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}