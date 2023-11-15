using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCharger.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedNameToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");
        }
    }
}
