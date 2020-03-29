using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class EmailSettingsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActivateEmailSenderSettings",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateFileSettings",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateEmailSenderSettings",
                table: "OrganizationDepartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivateFileSettings",
                table: "OrganizationDepartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone1",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone2",
                table: "OrganizationDepartments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivateEmailSenderSettings",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ActivateFileSettings",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ActivateEmailSenderSettings",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "ActivateFileSettings",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Phone1",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Phone2",
                table: "OrganizationDepartments");
        }
    }
}
