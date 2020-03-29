using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class IncidenceStatusUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncidenceStatuses_Organizations_OrganizationId",
                table: "IncidenceStatuses");

            migrationBuilder.DropIndex(
                name: "IX_IncidenceStatuses_OrganizationId",
                table: "IncidenceStatuses");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "IncidenceStatuses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "IncidenceStatuses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceStatuses_OrganizationId",
                table: "IncidenceStatuses",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncidenceStatuses_Organizations_OrganizationId",
                table: "IncidenceStatuses",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
