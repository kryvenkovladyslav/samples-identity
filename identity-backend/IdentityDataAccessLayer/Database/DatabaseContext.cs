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

        public DatabaseContext(IOptionsMonitor<ConnectionStringOptions> connectionOptions) 
        {
            this.connectionStringOptions = connectionOptions.CurrentValue;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connectionString: this.connectionStringOptions.ConnectionString, 
                sql => sql.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));
        }
    }
}