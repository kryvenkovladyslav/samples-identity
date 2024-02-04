using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Microsoft.AspNetCore.DataProtection;
using WebApplication.Infrastructure.Middleware;
using WebApplication.Infrastructure.Authentication;

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
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseMiddleware<CookieAuthenticationMiddleware>();
            app.UseAuthentication();
            app.UseMiddleware<RoleMembershipMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ClaimsReporterMiddleware>();
            //app.UseMiddleware<AuthorizationMiddleware>();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}