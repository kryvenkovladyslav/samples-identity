using System;
using Identity.Abstract.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.System.Database.Configuration
{
    public sealed class IdentityRoleConfiguration<TRole, TKey> : IEntityTypeConfiguration<TRole>
        where TKey : IEquatable<TKey>
        where TRole : IdentitySystemRole<TKey>
    {
        public void Configure(EntityTypeBuilder<TRole> builder)
        {
            builder.ToTable("IdentityRole");
            builder.HasKey(role => role.ID);
            builder.Property(role => role.Name).IsRequired(true);
            builder.Property(role => role.NormalizedName).IsRequired(true);
            builder.Property(role => role.ConcurrencyStamp).IsRequired(true);
        }
    }
}