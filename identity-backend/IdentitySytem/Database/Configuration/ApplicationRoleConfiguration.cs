using IdentitySystem.Database.Configuration.User;
using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace IdentitySystem.Database.Configuration
{
    public class ApplicationRoleConfiguration<TRole, TKey> : IEntityTypeConfiguration<TRole>
        where TKey : IEquatable<TKey>
        where TRole : BaseApplicationRole<TKey>, new()
    {
        public void Configure(EntityTypeBuilder<TRole> builder)
        {
            var roleTable = builder.ToTable("IdentityRole");

            roleTable.HasKey(role => role.ID);

            roleTable.Property(role => role.Name)
                .IsRequired(true);

            roleTable.Property(role => role.NormalizedName)
                .IsRequired(true);

            roleTable.Property(role => role.ConcurrencyStamp)
                .IsRequired(true);
        }
    }
}