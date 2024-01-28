using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartCharger.Data.Migrations
{
    /// <inheritdoc />
    public partial class newUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreationTime", "Email", "FirstName", "LastName", "Password", "RoleId", "Salt" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 9, 15, 14, 19, 692, DateTimeKind.Utc).AddTicks(8005), "admin@gmail.com", "Admin", "Admin", "$2b$10$hmyMVezD9FUJdULq3sFnI.oWajZxHcuWusLmIdCUTcg5XI9Zy4R3a", 1, "$2b$10$f1wBguLF9ane/U9yySuKau" },
                    { 2, true, new DateTime(2024, 1, 9, 15, 14, 19, 692, DateTimeKind.Utc).AddTicks(8012), "customer@gmail.com", "Customer", "Customer", "$2b$10$Zeaai0Ju24WY/x.cpZpFiOCZAyjdphShUUqCIeoPpPEn.LPBDB1uy", 2, "$2b$10$Zeaai0Ju24WY/x.cpZpFiO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
