using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultAdminUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "ADMIN_001",
                column: "PasswordHash",
                value: "$2a$12$rNjQUYl1Ap4ZrS/IuZeJ0uXkio2fo8esy298m6LzCqr32AqTJJbHq2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "ADMIN_001",
                column: "PasswordHash",
                value: "$2a$12$rNiQUYl1Ap4ZrS/IuZeJOuXkio2fo8esv298m6LzCqr32AgTJbHq2");
        }
    }
}
