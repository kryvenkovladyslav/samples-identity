﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication.Database;

#nullable disable

namespace IdentityDataAccessLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IdentityDataAccessLayer.Models.ApplicationRole", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("IdentityRole", (string)null);
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailAddressConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("NormalizedEmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("IdentityUser", (string)null);
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.ApplicationUserClaim", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("IdentityUserClaim", (string)null);
                });

            modelBuilder.Entity("IdentitySystem.Abstract.Models.IdentitySystemUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserID", "RoleID");

                    b.ToTable("IdentityUserRole", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.ApplicationUserRole", b =>
                {
                    b.HasBaseType("IdentitySystem.Abstract.Models.IdentitySystemUserRole<System.Guid>");

                    b.HasIndex("RoleID");

                    b.ToTable("ApplicationUserRole", (string)null);
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.ApplicationUserClaim", b =>
                {
                    b.HasOne("IdentityDataAccessLayer.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.ApplicationUserRole", b =>
                {
                    b.HasOne("IdentityDataAccessLayer.Models.ApplicationRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IdentityDataAccessLayer.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IdentitySystem.Abstract.Models.IdentitySystemUserRole<System.Guid>", null)
                        .WithOne()
                        .HasForeignKey("IdentityDataAccessLayer.Models.ApplicationUserRole", "UserID", "RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
