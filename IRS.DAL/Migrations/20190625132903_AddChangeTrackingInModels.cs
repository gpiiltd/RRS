using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class AddChangeTrackingInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "UserDeployment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "UserDeployment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdited",
                table: "UserDeployment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "UserDeployment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EditedByUserId",
                table: "UserDeployment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Protected",
                table: "UserDeployment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Protected",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "OrganizationDepartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Protected",
                table: "OrganizationDepartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Incidences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Protected",
                table: "Incidences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdited",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EditedByUserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Protected",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserDeployment_CreatedByUserId",
                table: "UserDeployment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeployment_EditedByUserId",
                table: "UserDeployment",
                column: "EditedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidences_CreatedByUserId",
                table: "Incidences",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidences_EditedByUserId",
                table: "Incidences",
                column: "EditedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_AspNetUsers_CreatedByUserId",
                table: "Incidences",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_AspNetUsers_EditedByUserId",
                table: "Incidences",
                column: "EditedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeployment_AspNetUsers_CreatedByUserId",
                table: "UserDeployment",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeployment_AspNetUsers_EditedByUserId",
                table: "UserDeployment",
                column: "EditedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_AspNetUsers_CreatedByUserId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_AspNetUsers_EditedByUserId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeployment_AspNetUsers_CreatedByUserId",
                table: "UserDeployment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeployment_AspNetUsers_EditedByUserId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_UserDeployment_CreatedByUserId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_UserDeployment_EditedByUserId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_CreatedByUserId",
                table: "Incidences");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_EditedByUserId",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "DateEdited",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "EditedByUserId",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "Protected",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Protected",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Protected",
                table: "OrganizationDepartments");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "Protected",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateEdited",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EditedByUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Protected",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
