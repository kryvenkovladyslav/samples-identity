﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication.Database;

#nullable disable

namespace IdentityDataAccessLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240215184336_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IdentityDataAccessLayer.Models.IdentityUser", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("EmailAddress");

                    b.Property<string>("IsEmailAddressConfirmed")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NVARCHAR")
                        .HasDefaultValue("0")
                        .HasColumnName("IsEmailAddressConfirmed");

                    b.Property<bool>("IsPhoneNumberConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(false)
                        .HasColumnName("IsPhoneNumberConfirmed");

                    b.Property<string>("NormalizedEmailAddress")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("NormalizedEmailAddress");

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("NormalizedUserName");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("PhoneNumber");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("UserName");

                    b.HasKey("ID");

                    b.ToTable("IdentityUser", (string)null);
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.IdentityUserClaim", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("ClaimType");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("ClaimValue");

                    b.Property<Guid>("UserID")
                        .HasColumnType("UNIQUEIDENTIFIER")
                        .HasColumnName("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("IdentityUserClaim", (string)null);
                });

            modelBuilder.Entity("IdentityDataAccessLayer.Models.IdentityUserClaim", b =>
                {
                    b.HasOne("IdentityDataAccessLayer.Models.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
