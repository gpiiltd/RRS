using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class IncidenceReporterFieldUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReporterFeedbackComment",
                table: "Incidences",
                newName: "ReporterFirstResponderAction");

            migrationBuilder.AddColumn<int>(
                name: "ManagerFeedbackRating",
                table: "Incidences",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReporterDepartmentId",
                table: "Incidences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerFeedbackRating",
                table: "Incidences");

            migrationBuilder.DropColumn(
                name: "ReporterDepartmentId",
                table: "Incidences");

            migrationBuilder.RenameColumn(
                name: "ReporterFirstResponderAction",
                table: "Incidences",
                newName: "ReporterFeedbackComment");
        }
    }
}
