using IdentitySystem.Database.Configuration;
using IdentitySystem.Database.Configuration.User;
using IdentitySystem.Database.Configuration.UserClaim;
using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentitySystem.Database
{
    public class IdentityDatabaseContext<TUser, TRole, TUserRole, TUserClaim, TKey> : DbContext
        where TKey: IEquatable<TKey>
        where TUser: BaseApplicationUser<TKey>, new()
        where TRole : BaseApplicationRole<TKey>, new()
        where TUserRole: BaseApplicationUserRole<TKey>, new()
        where TUserClaim : BaseApplicationUserClaim<TKey>, new()
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
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration<TUser, TKey>());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration<TRole, TKey>());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfiguration<TUser, TRole, TKey>());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfiguration<TUser, TUserClaim, TKey>());
        }
    }
}