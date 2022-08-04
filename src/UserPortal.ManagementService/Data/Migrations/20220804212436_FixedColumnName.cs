using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserPortal.ManagementService.Data.Migrations
{
    public partial class FixedColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviderSource",
                schema: "mngt",
                table: "UserManagements",
                newName: "SourceProvider");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                schema: "mngt",
                table: "UserManagements",
                newName: "SourceKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceProvider",
                schema: "mngt",
                table: "UserManagements",
                newName: "ProviderSource");

            migrationBuilder.RenameColumn(
                name: "SourceKey",
                schema: "mngt",
                table: "UserManagements",
                newName: "ProviderKey");
        }
    }
}
