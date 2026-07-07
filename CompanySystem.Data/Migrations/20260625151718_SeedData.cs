using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CreatedBy", "CreatedDate", "DepartmentName", "IsDeleted", "ManagerId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Human Resources", false, "admin", null, null },
                    { 2, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Information Technology", false, "admin", null, null }
                });

            migrationBuilder.InsertData(
                table: "MainPageSections",
                columns: new[] { "SectionId", "Content", "CreatedBy", "CreatedDate", "IsDeleted", "SectionType", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Welcome to Company System.", "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "About Us", null, null },
                    { 2, "Our professional services.", "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, "Services", null, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedBy", "CreatedDate", "IsDeleted", "RoleName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Admin", null, null },
                    { 2, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Manager", null, null },
                    { 3, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Employee", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MainPageSections",
                keyColumn: "SectionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MainPageSections",
                keyColumn: "SectionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3);
        }
    }
}
