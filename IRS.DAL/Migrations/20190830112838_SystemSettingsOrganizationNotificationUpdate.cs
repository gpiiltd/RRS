using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class SystemSettingsOrganizationNotificationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailRecipientAddresses",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSRecipientNumbers",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSSenderName",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailRecipientAddresses",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSRecipientNumbers",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSSenderName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSSenderName",
                table: "OrganizationDepartments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailRecipientAddresses",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "SMSRecipientNumbers",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "SMSSenderName",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "EmailRecipientAddresses",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SMSRecipientNumbers",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SMSSenderName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SMSSenderName",
                table: "OrganizationDepartments");
        }
    }
}
