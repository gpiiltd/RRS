using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class FileUploadChannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Photos",
                newName: "Uri");

            migrationBuilder.AlterColumn<int>(
                name: "PageSize",
                table: "SystemSettings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "MaxFileSize",
                table: "SystemSettings",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdited",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUploaded",
                table: "Photos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FileUploadChannel",
                table: "Photos",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PageSize",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "MaxFileSize",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEdited",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DateUploaded",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "FileUploadChannel",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Photos",
                newName: "Url");

            migrationBuilder.AlterColumn<int>(
                name: "PageSize",
                table: "SystemSettings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaxFileSize",
                table: "SystemSettings",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PageSize",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxFileSize",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
