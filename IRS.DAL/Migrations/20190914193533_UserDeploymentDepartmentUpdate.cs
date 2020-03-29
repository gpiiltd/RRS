using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class UserDeploymentDepartmentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_OrganizationDepartments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeployment_Organizations_OrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeployment_Organizations_UserOrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_UserDeployment_OrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_UserDeployment_UserOrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "UserOrganizationId",
                table: "UserDeployment");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "UserDeployment",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserOrganizationId",
                table: "UserDeployment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDeployment_OrganizationId",
                table: "UserDeployment",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeployment_UserOrganizationId",
                table: "UserDeployment",
                column: "UserOrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_OrganizationDepartments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "OrganizationDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeployment_Organizations_OrganizationId",
                table: "UserDeployment",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeployment_Organizations_UserOrganizationId",
                table: "UserDeployment",
                column: "UserOrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
