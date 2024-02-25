using System;
using Identity.System.Database;
using IdentityDataAccessLayer.Models;
using IdentityDataAccessLayer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApplication.Database
{
    public sealed class DatabaseContext :
        IdentityDatabaseContext<ApplicationUser, ApplicationRole, ApplicationUserRole, ApplicationUserClaim, Guid>
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public DatabaseContext()  { }

         public DatabaseContext(IOptionsMonitor<ConnectionStringOptions> connectionOptions) 
         {
             this.connectionStringOptions = connectionOptions.CurrentValue;
         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var userRoleTable = modelBuilder.Entity<ApplicationUserRole>().ToTable(nameof(ApplicationUserRole));
            userRoleTable.HasOne(x => x.Role).WithMany().HasForeignKey(userRole => userRole.RoleID).HasPrincipalKey(role => role.ID);
            userRoleTable.HasOne(x => x.User).WithMany().HasForeignKey(userRole => userRole.UserID).HasPrincipalKey(user => user.ID);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=IdentityCustomDatabase;Trusted_Connection=True;TrustServerCertificate=True;", 
                sql => sql.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));
        }
    }
}