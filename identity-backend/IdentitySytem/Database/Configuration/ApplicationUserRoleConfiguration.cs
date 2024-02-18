using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IdentitySystem.Database.Configuration
{
    public class ApplicationUserRoleConfiguration<TUser, TRole, TKey> : IEntityTypeConfiguration<BaseApplicationUserRole<TKey>>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>, new()
        where TRole : BaseApplicationRole<TKey>, new()
    {
        public void Configure(EntityTypeBuilder<BaseApplicationUserRole<TKey>> builder)
        {
            var userRoleTable = builder.ToTable("IdentityUserRole");

            userRoleTable.HasKey(userRole => new { userRole.UserID, userRole.RoleID });
            userRoleTable.HasOne<TUser>().WithMany().HasForeignKey(userRole => userRole.UserID).HasPrincipalKey(user => user.ID);
            userRoleTable.HasOne<TRole>().WithMany().HasForeignKey(userRole => userRole.RoleID).HasPrincipalKey(role => role.ID);
        }
    }
}