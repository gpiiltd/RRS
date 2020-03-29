using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class OrganizationMultitenancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_OrganizationDepartments_DepartmentId",
                table: "Incidences");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Incidences",
                newName: "AssignedOrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Incidences_DepartmentId",
                table: "Incidences",
                newName: "IX_Incidences_AssignedOrganizationId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserOrganizationId",
                table: "UserDeployment",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "IncidenceTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "IncidenceTypeDepartmentMappings",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "IncidenceStatuses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedDepartmentId",
                table: "Incidences",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Gallery",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "AuditTrails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDeployment_UserOrganizationId",
                table: "UserDeployment",
                column: "UserOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceTypes_OrganizationId",
                table: "IncidenceTypes",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceTypeDepartmentMappings_OrganizationId",
                table: "IncidenceTypeDepartmentMappings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceStatuses_OrganizationId",
                table: "IncidenceStatuses",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidences_AssignedDepartmentId",
                table: "Incidences",
                column: "AssignedDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_OrganizationId",
                table: "Gallery",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_OrganizationId",
                table: "AuditTrails",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrails_Organizations_OrganizationId",
                table: "AuditTrails",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gallery_Organizations_OrganizationId",
                table: "Gallery",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_OrganizationDepartments_AssignedDepartmentId",
                table: "Incidences",
                column: "AssignedDepartmentId",
                principalTable: "OrganizationDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_Organizations_AssignedOrganizationId",
                table: "Incidences",
                column: "AssignedOrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidenceStatuses_Organizations_OrganizationId",
                table: "IncidenceStatuses",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidenceTypeDepartmentMappings_Organizations_OrganizationId",
                table: "IncidenceTypeDepartmentMappings",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidenceTypes_Organizations_OrganizationId",
                table: "IncidenceTypes",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrails_Organizations_OrganizationId",
                table: "AuditTrails");

            migrationBuilder.DropForeignKey(
                name: "FK_Gallery_Organizations_OrganizationId",
                table: "Gallery");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_OrganizationDepartments_AssignedDepartmentId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_Organizations_AssignedOrganizationId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidenceStatuses_Organizations_OrganizationId",
                table: "IncidenceStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidenceTypeDepartmentMappings_Organizations_OrganizationId",
                table: "IncidenceTypeDepartmentMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidenceTypes_Organizations_OrganizationId",
                table: "IncidenceTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeployment_Organizations_UserOrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_UserDeployment_UserOrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropIndex(
                name: "IX_IncidenceTypes_OrganizationId",
                table: "IncidenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_IncidenceTypeDepartmentMappings_OrganizationId",
                table: "IncidenceTypeDepartmentMappings");

            migrationBuilder.DropIndex(
                name: "IX_IncidenceStatuses_OrganizationId",
                table: "IncidenceStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_AssignedDepartmentId",
                table: "Incidences");

            migrationBuilder.DropIndex(
                name: "IX_Gallery_OrganizationId",
                table: "Gallery");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_OrganizationId",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "UserOrganizationId",
                table: "UserDeployment");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "IncidenceTypes");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "IncidenceTypeDepartmentMappings");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "IncidenceStatuses");

            migrationBuilder.DropColumn(
                name: "AssignedDepartmentId",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "AssignedOrganizationId",
                table: "Incidences",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Incidences_AssignedOrganizationId",
                table: "Incidences",
                newName: "IX_Incidences_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_OrganizationDepartments_DepartmentId",
                table: "Incidences",
                column: "DepartmentId",
                principalTable: "OrganizationDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
