using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class UserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OrganizationDepartments_OrganizationDepartmentId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OrganizationDepartmentId",
                table: "AspNetUsers",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_OrganizationDepartmentId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OrganizationDepartments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "OrganizationDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OrganizationDepartments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "AspNetUsers",
                newName: "OrganizationDepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_OrganizationDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OrganizationDepartments_OrganizationDepartmentId",
                table: "AspNetUsers",
                column: "OrganizationDepartmentId",
                principalTable: "OrganizationDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
