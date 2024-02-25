using System;
using Identity.Abstract.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.System.Database.Configuration
{
    public sealed class IdentityUserClaimConfiguration<TUser, TUserClaim, TKey> : IEntityTypeConfiguration<TUserClaim>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
        where TUserClaim : IdentitySystemUserClaim<TKey>
    {
        public void Configure(EntityTypeBuilder<TUserClaim> builder)
        {
            builder.ToTable("IdentityUserClaim");
            builder.HasKey(userClaim => userClaim.ID);
            builder.Property(userClaim => userClaim.UserID).IsRequired(true);
            builder.Property(userClaim => userClaim.ClaimValue).IsRequired(true);
            builder.Property(userClaim => userClaim.ClaimType).IsRequired(true);
            builder.HasOne<TUser>().WithMany().HasForeignKey(u => u.UserID).HasPrincipalKey(user => user.ID);
        }
    }
}