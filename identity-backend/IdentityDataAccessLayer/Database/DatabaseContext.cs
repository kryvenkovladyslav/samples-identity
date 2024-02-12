using IdentityDataAccessLayer.Models;
using IdentityDataAccessLayer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApplication.Database
{
    public sealed class DatabaseContext : DbContext
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<IdentityUserClaim> Claims { get; set; }


        // public DatabaseContext(DbContextOptions<DatabaseContext> options) :base(options) { }
        /* public DatabaseContext(IOptionsMonitor<ConnectionStringOptions> connectionOptions) 
         {
             this.connectionStringOptions = connectionOptions.CurrentValue;
         }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connectionString: "Server=localhost;Database=CustomIdentity;Trusted_Connection=True;TrustServerCertificate=True;", 
                sql => sql.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));
        }
    }
}