using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class IncidenceFieldsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_IncidenceStatuses_IncidenceStatusId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_IncidenceTypes_IncidenceTypeId",
                table: "Incidences");

            migrationBuilder.AlterColumn<Guid>(
                name: "IncidenceTypeId",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(

                name: "IncidenceStatusId",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_IncidenceStatuses_IncidenceStatusId",
                table: "Incidences",
                column: "IncidenceStatusId",
                principalTable: "IncidenceStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_IncidenceTypes_IncidenceTypeId",
                table: "Incidences",
                column: "IncidenceTypeId",
                principalTable: "IncidenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_IncidenceStatuses_IncidenceStatusId",
                table: "Incidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidences_IncidenceTypes_IncidenceTypeId",
                table: "Incidences");

            migrationBuilder.AlterColumn<Guid>(
                name: "IncidenceTypeId",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IncidenceStatusId",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_IncidenceStatuses_IncidenceStatusId",
                table: "Incidences",
                column: "IncidenceStatusId",
                principalTable: "IncidenceStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidences_IncidenceTypes_IncidenceTypeId",
                table: "Incidences",
                column: "IncidenceTypeId",
                principalTable: "IncidenceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
