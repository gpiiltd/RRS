using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class DepartmentNotificationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptedImageFileTypes",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptedVideoFileTypes",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email1",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email2",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailRecipientAddresses",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSenderName",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSenderPassword",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSendersEmail",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostName",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxImageFileSize",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxVideoFileSize",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageSize",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Port",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSRecipientNumbers",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendSMS",
                table: "OrganizationDepartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SmsServiceProvider",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseSsl",
                table: "OrganizationDepartments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedImageFileTypes",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "AcceptedVideoFileTypes",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Email1",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Email2",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "EmailRecipientAddresses",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "EmailSenderName",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "EmailSenderPassword",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "EmailSendersEmail",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "HostName",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "MaxImageFileSize",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "MaxVideoFileSize",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "PageSize",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "SMSRecipientNumbers",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "SendSMS",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "SmsServiceProvider",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "UseSsl",
                table: "OrganizationDepartments");
        }
    }
}
