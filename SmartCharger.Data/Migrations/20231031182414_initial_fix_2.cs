using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCharger.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial_fix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Users",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Chargers",
                newName: "CreationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Users",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Chargers",
                newName: "Created");
        }
    }
}
