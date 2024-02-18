using IdentityDataAccessLayer.Models;
using IdentityDataAccessLayer.Options;
using IdentitySystem.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace WebApplication.Database
{
    public sealed class DatabaseContext : IdentityDatabaseContext<IdentityUser, IdentityRole, IdentityUserRole, IdentityUserClaim, Guid>
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

            var userRoleTable = modelBuilder.Entity<IdentityUserRole>().ToTable(nameof(IdentityUserRole));
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