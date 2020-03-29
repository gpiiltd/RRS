using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class NotificationBySMSUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailSenderName",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSenderPassword",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSendersEmail",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendSMS",
                table: "SystemSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SmsServiceProvider",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSenderName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSenderPassword",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSendersEmail",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendSMS",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SmsServiceProvider",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSenderName",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "EmailSenderPassword",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "EmailSendersEmail",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "SendSMS",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "SmsServiceProvider",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "EmailSenderName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EmailSenderPassword",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EmailSendersEmail",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SendSMS",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SmsServiceProvider",
                table: "Organizations");
        }
    }
}
