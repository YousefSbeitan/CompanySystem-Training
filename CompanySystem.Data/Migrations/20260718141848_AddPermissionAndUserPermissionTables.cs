using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionAndUserPermissionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.UserPermissionId);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

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
                table: "Permissions",
                columns: new[] { "PermissionId", "CreatedBy", "CreatedDate", "Description", "IsDeleted", "PermissionName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View departments", false, "Departments.View", null, null },
                    { 2, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create departments", false, "Departments.Create", null, null },
                    { 3, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edit departments", false, "Departments.Edit", null, null },
                    { 4, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete departments", false, "Departments.Delete", null, null },
                    { 5, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View users", false, "Users.View", null, null },
                    { 6, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create users", false, "Users.Create", null, null },
                    { 7, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edit users", false, "Users.Edit", null, null },
                    { 8, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete users", false, "Users.Delete", null, null },
                    { 9, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View roles", false, "Roles.View", null, null },
                    { 10, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create roles", false, "Roles.Create", null, null },
                    { 11, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edit roles", false, "Roles.Edit", null, null },
                    { 12, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete roles", false, "Roles.Delete", null, null },
                    { 13, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View notes", false, "Notes.View", null, null },
                    { 14, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create notes", false, "Notes.Create", null, null },
                    { 15, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edit notes", false, "Notes.Edit", null, null },
                    { 16, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete notes", false, "Notes.Delete", null, null },
                    { 17, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View main page sections", false, "MainPageSections.View", null, null },
                    { 18, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create main page sections", false, "MainPageSections.Create", null, null },
                    { 19, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edit main page sections", false, "MainPageSections.Edit", null, null },
                    { 20, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete main page sections", false, "MainPageSections.Delete", null, null },
                    { 21, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View permissions", false, "Permissions.View", null, null },
                    { 22, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create permissions", false, "Permissions.Create", null, null },
                    { 23, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edit permissions", false, "Permissions.Edit", null, null },
                    { 24, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete permissions", false, "Permissions.Delete", null, null },
                    { 25, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View dashboard", false, "Dashboard.View", null, null }
                });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "UserPermissionId", "CreatedBy", "CreatedDate", "IsDeleted", "PermissionId", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, null, null, "ADMIN_001" },
                    { 2, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, null, null, "ADMIN_001" },
                    { 3, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, null, null, "ADMIN_001" },
                    { 4, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 4, null, null, "ADMIN_001" },
                    { 5, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 5, null, null, "ADMIN_001" },
                    { 6, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6, null, null, "ADMIN_001" },
                    { 7, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 7, null, null, "ADMIN_001" },
                    { 8, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 8, null, null, "ADMIN_001" },
                    { 9, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 9, null, null, "ADMIN_001" },
                    { 10, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 10, null, null, "ADMIN_001" },
                    { 11, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 11, null, null, "ADMIN_001" },
                    { 12, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 12, null, null, "ADMIN_001" },
                    { 13, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 13, null, null, "ADMIN_001" },
                    { 14, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 14, null, null, "ADMIN_001" },
                    { 15, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 15, null, null, "ADMIN_001" },
                    { 16, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 16, null, null, "ADMIN_001" },
                    { 17, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 17, null, null, "ADMIN_001" },
                    { 18, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 18, null, null, "ADMIN_001" },
                    { 19, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 19, null, null, "ADMIN_001" },
                    { 20, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 20, null, null, "ADMIN_001" },
                    { 21, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 21, null, null, "ADMIN_001" },
                    { 22, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 22, null, null, "ADMIN_001" },
                    { 23, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 23, null, null, "ADMIN_001" },
                    { 24, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 24, null, null, "ADMIN_001" },
                    { 25, "System", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 25, null, null, "ADMIN_001" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

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
