using System;
using Identity.Abstract.Models;
using Microsoft.EntityFrameworkCore;
using Identity.System.Database.Configuration;

namespace Identity.System.Database
{
    public class IdentityDatabaseContext<TUser, TRole, TUserRole, TUserClaim, TKey> : DbContext
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
        where TRole : IdentitySystemRole<TKey>
        where TUserRole : IdentitySystemUserRole<TKey>
        where TUserClaim : IdentitySystemUserClaim<TKey>
    {
        public virtual DbSet<TUser> Users { get; set; }

        public virtual DbSet<TRole> Roles { get; set; }

        public virtual DbSet<TUserRole> UserRoles { get; set; }

        public virtual DbSet<TUserClaim> UserClaims { get; set; }

        public IdentityDatabaseContext() { }

        public IdentityDatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new IdentityUserConfiguration<TUser, TKey>());
            modelBuilder.ApplyConfiguration(new IdentityRoleConfiguration<TRole, TKey>());
            modelBuilder.ApplyConfiguration(new IdentityUserRoleConfiguration<TUser, TRole, TKey>());
            modelBuilder.ApplyConfiguration(new IdentityUserClaimConfiguration<TUser, TUserClaim, TKey>());
        }
    }
}