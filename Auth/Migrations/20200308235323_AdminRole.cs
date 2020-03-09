using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "18804585-8c97-4c86-9f8a-6e147b2a3c53", "5a5d653a-e5e9-4ac3-8353-bac1d54775c9", "admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18804585-8c97-4c86-9f8a-6e147b2a3c53");
        }
    }
}
