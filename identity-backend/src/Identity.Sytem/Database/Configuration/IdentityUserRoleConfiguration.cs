using System;
using Identity.Abstract.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.System.Database.Configuration
{
    public sealed class IdentityUserRoleConfiguration<TUser, TRole, TKey> : IEntityTypeConfiguration<IdentitySystemUserRole<TKey>>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
        where TRole : IdentitySystemRole<TKey>
    {
        public void Configure(EntityTypeBuilder<IdentitySystemUserRole<TKey>> builder)
        {
            builder.ToTable("IdentityUserRole");
            builder.HasKey(userRole => new { userRole.UserID, userRole.RoleID });
            builder.HasOne<TUser>().WithMany().HasForeignKey(userRole => userRole.UserID).HasPrincipalKey(user => user.ID);
            builder.HasOne<TRole>().WithMany().HasForeignKey(userRole => userRole.RoleID).HasPrincipalKey(role => role.ID);
        }
    }
}