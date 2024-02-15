﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityDataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "NVARCHAR", nullable: false),
                    EmailAddress = table.Column<string>(type: "NVARCHAR", nullable: false),
                    NormalizedEmailAddress = table.Column<string>(type: "NVARCHAR", nullable: false),
                    IsEmailAddressConfirmed = table.Column<string>(type: "NVARCHAR(1)", nullable: false, defaultValue: "0"),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR", nullable: false),
                    IsPhoneNumberConfirmed = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR", nullable: false),
                    ClaimValue = table.Column<string>(type: "NVARCHAR", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim_IdentityUser_UserID",
                        column: x => x.UserID,
                        principalTable: "IdentityUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_UserID",
                table: "IdentityUserClaim",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUserClaim");

            migrationBuilder.DropTable(
                name: "IdentityUser");
        }
    }
}
