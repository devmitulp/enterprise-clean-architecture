using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EnableJobTitleAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Drop foreign key constraint
                ALTER TABLE Employees DROP CONSTRAINT FK_Employees_JobTitles_JobTitleId;
                
                -- 2. Drop primary key constraint
                ALTER TABLE JobTitles DROP CONSTRAINT PK_JobTitles;
                
                -- 3. Rename current table
                EXEC sp_rename 'JobTitles', 'Tmp_JobTitles';
                
                -- 4. Create new table with IDENTITY property
                CREATE TABLE JobTitles (
                    Id INT IDENTITY(1,1) NOT NULL,
                    Name NVARCHAR(200) NOT NULL,
                    Description NVARCHAR(500) NULL,
                    IsActive BIT NOT NULL DEFAULT 1,
                    CreatedDateUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    CreatedBy INT NULL,
                    UpdatedDateUtc DATETIME2 NULL,
                    UpdatedBy INT NULL,
                    IsDeleted BIT NOT NULL DEFAULT 0,
                    CONSTRAINT PK_JobTitles PRIMARY KEY CLUSTERED (Id)
                );
                
                -- 5. Copy data using IDENTITY_INSERT to preserve existing IDs
                SET IDENTITY_INSERT JobTitles ON;
                IF EXISTS(SELECT * FROM Tmp_JobTitles)
                BEGIN
                    INSERT INTO JobTitles (Id, Name, Description, IsActive, CreatedDateUtc, CreatedBy, UpdatedDateUtc, UpdatedBy, IsDeleted)
                    SELECT Id, Name, Description, IsActive, CreatedDateUtc, CreatedBy, UpdatedDateUtc, UpdatedBy, IsDeleted
                    FROM Tmp_JobTitles;
                END
                SET IDENTITY_INSERT JobTitles OFF;
                
                -- 6. Drop temporary table
                DROP TABLE Tmp_JobTitles;
                
                -- 7. Re-add foreign key constraint
                ALTER TABLE Employees ADD CONSTRAINT FK_Employees_JobTitles_JobTitleId
                    FOREIGN KEY (JobTitleId) REFERENCES JobTitles(Id) ON DELETE NO ACTION;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Drop foreign key constraint
                ALTER TABLE Employees DROP CONSTRAINT FK_Employees_JobTitles_JobTitleId;
                
                -- 2. Drop primary key constraint
                ALTER TABLE JobTitles DROP CONSTRAINT PK_JobTitles;
                
                -- 3. Rename current table
                EXEC sp_rename 'JobTitles', 'Tmp_JobTitles';
                
                -- 4. Create new table without IDENTITY property
                CREATE TABLE JobTitles (
                    Id INT NOT NULL,
                    Name NVARCHAR(200) NOT NULL,
                    Description NVARCHAR(500) NULL,
                    IsActive BIT NOT NULL DEFAULT 1,
                    CreatedDateUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    CreatedBy INT NULL,
                    UpdatedDateUtc DATETIME2 NULL,
                    UpdatedBy INT NULL,
                    IsDeleted BIT NOT NULL DEFAULT 0,
                    CONSTRAINT PK_JobTitles PRIMARY KEY CLUSTERED (Id)
                );
                
                -- 5. Copy data
                IF EXISTS(SELECT * FROM Tmp_JobTitles)
                BEGIN
                    INSERT INTO JobTitles (Id, Name, Description, IsActive, CreatedDateUtc, CreatedBy, UpdatedDateUtc, UpdatedBy, IsDeleted)
                    SELECT Id, Name, Description, IsActive, CreatedDateUtc, CreatedBy, UpdatedDateUtc, UpdatedBy, IsDeleted
                    FROM Tmp_JobTitles;
                END
                
                -- 6. Drop temporary table
                DROP TABLE Tmp_JobTitles;
                
                -- 7. Re-add foreign key constraint
                ALTER TABLE Employees ADD CONSTRAINT FK_Employees_JobTitles_JobTitleId
                    FOREIGN KEY (JobTitleId) REFERENCES JobTitles(Id) ON DELETE NO ACTION;
            ");
        }
    }
}
