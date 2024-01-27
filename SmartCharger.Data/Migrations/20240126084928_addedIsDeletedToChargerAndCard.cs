using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCharger.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedIsDeletedToChargerAndCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Chargers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2024, 1, 26, 8, 49, 28, 767, DateTimeKind.Utc).AddTicks(3748));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2024, 1, 26, 8, 49, 28, 767, DateTimeKind.Utc).AddTicks(3752));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Chargers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cards");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2024, 1, 9, 15, 14, 19, 692, DateTimeKind.Utc).AddTicks(8005));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2024, 1, 9, 15, 14, 19, 692, DateTimeKind.Utc).AddTicks(8012));
        }
    }
}
