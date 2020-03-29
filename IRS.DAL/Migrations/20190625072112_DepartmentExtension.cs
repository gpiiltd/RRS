using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class DepartmentExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrganizationDepartments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrganizationDepartments");
        }
    }
}
