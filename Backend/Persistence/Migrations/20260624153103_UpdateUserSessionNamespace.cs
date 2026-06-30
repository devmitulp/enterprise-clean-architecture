using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSessionNamespace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only rename if the old column name still exists (safe for fresh installs)
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'UserSessions')
                      AND name = N'AccessTokenExpirationTime'
                )
                BEGIN
                    EXEC sp_rename N'UserSessions.AccessTokenExpirationTime', N'AccessTokenExpiryTime', 'COLUMN';
                END
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessTokenExpiryTime",
                table: "UserSessions",
                newName: "AccessTokenExpirationTime");
        }
    }
}
