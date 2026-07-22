using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMfaPropertiesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MfaFailedAttempts",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MfaLockedUntilUtc",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MfaRecoveryCodes",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MfaSecret",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MfaFailedAttempts", "MfaLockedUntilUtc", "MfaRecoveryCodes", "MfaSecret" },
                values: new object[] { 0, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MfaFailedAttempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MfaLockedUntilUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MfaRecoveryCodes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MfaSecret",
                table: "Users");
        }
    }
}
