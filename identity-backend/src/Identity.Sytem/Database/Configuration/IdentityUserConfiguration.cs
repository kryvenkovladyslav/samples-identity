using System;
using Identity.Abstract.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.System.Database.Configuration
{
    public sealed class IdentityUserConfiguration<TUser, TKey> : IEntityTypeConfiguration<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        public void Configure(EntityTypeBuilder<TUser> builder)
        {
            builder.ToTable("IdentityUser");
            builder.HasKey(user => user.ID);
            builder.Property(user => user.UserName).IsRequired(true);
            builder.Property(user => user.NormalizedUserName).IsRequired(true);
            builder.Property(user => user.EmailAddress).IsRequired(true);
            builder.Property(user => user.NormalizedEmailAddress).IsRequired(true);
            builder.Property(user => user.IsEmailAddressConfirmed).IsRequired(true);
            builder.Property(user => user.PhoneNumber).IsRequired(true);
            builder.Property(user => user.IsPhoneNumberConfirmed).IsRequired(true);
            builder.Property(user => user.Password).IsRequired(true);
            builder.Property(user => user.SecurityStamp).IsRequired(false);
        }
    }
}