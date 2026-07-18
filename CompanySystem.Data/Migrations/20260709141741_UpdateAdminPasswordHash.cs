using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "ADMIN_001",
                column: "PasswordHash",
                value: "$2a$12$0abb8Tyz65y2IVbOZh8eReGMQNUXrk9L4.BbE5uNjWIp6dmXzml7q");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "ADMIN_001",
                column: "PasswordHash",
                value: "$2a$12$rNjQUYl1Ap4ZrS/IuZeJ0uXkio2fo8esy298m6LzCqr32AqTJJbHq2");
        }
    }
}
