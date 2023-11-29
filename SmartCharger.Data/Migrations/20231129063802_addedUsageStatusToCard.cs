using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCharger.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedUsageStatusToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsageStatus",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsageStatus",
                table: "Cards");
        }
    }
}
