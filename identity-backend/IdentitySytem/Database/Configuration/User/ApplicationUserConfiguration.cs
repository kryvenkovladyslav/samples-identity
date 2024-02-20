using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IdentitySystem.Database.Configuration.User
{
    public sealed class ApplicationUserConfiguration<TUser, TKey> : IEntityTypeConfiguration<TUser>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>, new()
    {
        public void Configure(EntityTypeBuilder<TUser> builder)
        {
            var userTable = builder.ToTable(ApplicationUserConfigurationDefaults.TableName);

            userTable.HasKey(user => user.ID);

            userTable.Property(user => user.UserName)
                .HasColumnName(ApplicationUserConfigurationDefaults.UserName)
                .IsRequired(true);

            userTable.Property(user => user.NormalizedUserName)
                .HasColumnName(ApplicationUserConfigurationDefaults.NormalizedUserName)
                .IsRequired(true);

            userTable.Property(user => user.EmailAddress)
                .HasColumnName(ApplicationUserConfigurationDefaults.EmailAddress)
                .IsRequired(true);

            userTable.Property(user => user.NormalizedEmailAddress)
                .HasColumnName(ApplicationUserConfigurationDefaults.NormalizedEmailAddress)
                .IsRequired(true);

            userTable.Property(user => user.IsEmailAddressConfirmed)
                .HasColumnName(ApplicationUserConfigurationDefaults.IsEmailAddressConfirmed)
                .HasDefaultValue(false)
                .IsRequired(true);

            userTable.Property(user => user.PhoneNumber)
                .HasColumnName(ApplicationUserConfigurationDefaults.PhoneNumber)
                .IsRequired(true);

            userTable.Property(user => user.IsPhoneNumberConfirmed)
                .HasColumnName(ApplicationUserConfigurationDefaults.IsPhoneNumberConfirmed)
                .HasDefaultValue(false)
                .IsRequired(true);

            userTable.Property(user => user.SecurityStamp)
                .HasColumnName(ApplicationUserConfigurationDefaults.SecurityStamp)
                .HasDefaultValue(string.Empty)
                .IsRequired(false);
        }
    }
}