using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IdentitySystem.Database.Configuration.UserClaim
{
    public class ApplicationUserClaimConfiguration<TUser, TUserClaim, TKey> : IEntityTypeConfiguration<TUserClaim>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
        where TUserClaim : BaseApplicationUserClaim<TKey>
    {
        public void Configure(EntityTypeBuilder<TUserClaim> builder)
        {
            var userClaimTable = builder.ToTable(ApplicationUserClaimConfigurationDefaults.TableName);

            userClaimTable.HasKey(userClaim => userClaim.ID);

            userClaimTable.Property(userClaim => userClaim.UserID)
                .HasColumnName(ApplicationUserClaimConfigurationDefaults.UserID)
                .HasColumnType(SqlServerTypes.Guid)
                .IsRequired(true);

            userClaimTable.Property(userClaim => userClaim.ClaimValue)
               .HasColumnName(ApplicationUserClaimConfigurationDefaults.ClaimValue)
               .HasColumnType(SqlServerTypes.String)
               .IsRequired(true);

            userClaimTable.Property(userClaim => userClaim.ClaimType)
               .HasColumnName(ApplicationUserClaimConfigurationDefaults.ClaimType)
               .HasColumnType(SqlServerTypes.String)
               .IsRequired(true);

            userClaimTable.HasOne<TUser>().WithMany().HasForeignKey(u => u.UserID).HasPrincipalKey(user => user.ID);
        }
    }
}