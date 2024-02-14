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

            userTable.HasKey(user => user.ID).HasName(ApplicationUserConfigurationDefaults.ID);

            userTable.Property(user => user.UserName)
                .HasColumnName(ApplicationUserConfigurationDefaults.UserName)
                .HasColumnType(SqlServerTypes.String)
                .IsRequired(true);

            userTable.Property(user => user.NormalizedUserName)
                .HasColumnName(ApplicationUserConfigurationDefaults.NormalizedUserName)
                .HasColumnType(SqlServerTypes.String)
                .IsRequired(true);

            userTable.Property(user => user.EmailAddress)
                .HasColumnName(ApplicationUserConfigurationDefaults.EmailAddress)
                .HasColumnType(SqlServerTypes.String)
                .IsRequired(true);

            userTable.Property(user => user.NormalizedEmailAddress)
                .HasColumnName(ApplicationUserConfigurationDefaults.NormalizedEmailAddress)
                .HasColumnType(SqlServerTypes.String)
                .IsRequired(true);

            userTable.Property(user => user.IsEmailAddressConfirmed)
                .HasColumnName(ApplicationUserConfigurationDefaults.IsEmailAddressConfirmed)
                .HasColumnType(SqlServerTypes.String)
                .HasDefaultValue(false)
                .IsRequired(true);

            userTable.Property(user => user.PhoneNumber)
                .HasColumnName(ApplicationUserConfigurationDefaults.PhoneNumber)
                .HasColumnType(SqlServerTypes.String)
                .IsRequired(true);

            userTable.Property(user => user.IsPhoneNumberConfirmed)
                .HasColumnName(ApplicationUserConfigurationDefaults.IsPhoneNumberConfirmed)
                .HasColumnType(SqlServerTypes.Boolean)
                .HasDefaultValue(false)
                .IsRequired(true);
        }
    }
}