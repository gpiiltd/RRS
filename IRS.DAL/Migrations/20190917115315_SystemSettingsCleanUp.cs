using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class SystemSettingsCleanUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_SystemSettings_SystemSettingId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationDepartments_SystemSettings_SystemSettingId",
                table: "OrganizationDepartments");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationDepartments_SystemSettingId",
                table: "OrganizationDepartments");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_SystemSettingId",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "SystemSettingId",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "SystemSettingId",
                table: "Incidences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SystemSettingId",
                table: "OrganizationDepartments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SystemSettingId",
                table: "Incidences",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationDepartments_SystemSettingId",
                table: "OrganizationDepartments",
                column: "SystemSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidences_SystemSettingId",
                table: "Incidences",
                column: "SystemSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_SystemSettings_SystemSettingId",
                table: "Incidences",
                column: "SystemSettingId",
                principalTable: "SystemSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationDepartments_SystemSettings_SystemSettingId",
                table: "OrganizationDepartments",
                column: "SystemSettingId",
                principalTable: "SystemSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
