using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class IncidenceOrganizationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Incidences",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidences_DepartmentId",
                table: "Incidences",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_OrganizationDepartments_DepartmentId",
                table: "Incidences",
                column: "DepartmentId",
                principalTable: "OrganizationDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_OrganizationDepartments_DepartmentId",
                table: "Incidences");

            migrationBuilder.DropIndex(
                name: "IX_Incidences_DepartmentId",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Incidences");
        }
    }
}
