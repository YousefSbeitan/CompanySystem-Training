using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "ManagerId",
                value: "ADMIN_001");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "ManagerId",
                value: "ADMIN_001");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedBy", "CreatedDate", "DepartmentId", "IsActive", "IsDeleted", "LeaderId", "PasswordHash", "PhoneNumber", "RoleId", "Salary", "StartDate", "UpdatedBy", "UpdatedDate", "Username" },
                values: new object[] { "ADMIN_001", "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, false, "ADMIN_001", "$2a$12$rNiQUYl1Ap4ZrS/IuZeJOuXkio2fo8esv298m6LzCqr32AgTJbHq2", "0000000000", 1, 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "ADMIN_001");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "ManagerId",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "ManagerId",
                value: "admin");
        }
    }
}
