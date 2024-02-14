using IdentityDataAccessLayer.Models;
using IdentityDataAccessLayer.Options;
using IdentitySystem.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace WebApplication.Database
{
    public sealed class DatabaseContext : IdentityDatabaseContext<IdentityUser, IdentityUserClaim, Guid>
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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=CustomIdentityCustom;Trusted_Connection=True;TrustServerCertificate=True;", 
                sql => sql.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));
        }
    }
}