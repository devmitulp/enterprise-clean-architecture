using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAndUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "Email", "EmployeeCode", "FirstName", "IsActive", "IsDeleted", "JobTitleId", "LastName", "UpdatedBy", "UpdatedDateUtc" },
                values: new object[] { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mitul@company.com", "EMP001", "Mitul", true, false, 1, "Patel", null, null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "Email", "EmployeeId", "ExternalProviderId", "IsActive", "IsDeleted", "LastLoginDateUtc", "LoginProvider", "PasswordHash", "UpdatedBy", "UpdatedDateUtc", "UserName" },
                values: new object[] { 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@company.com", 1, null, true, false, null, 0, "AQAAAAIAAYagAAAAEOEICcDos33D5KeqhPKlST+y37hWet2yDs9KQL4GLEWLrhiJZ4EFQtr5uBkOVtGpzw==", null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
