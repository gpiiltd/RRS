using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class IncidenceTypeDepartmentMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncidenceTypeDepartmentMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IncidenceTypeId = table.Column<Guid>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidenceTypeDepartmentMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidenceTypeDepartmentMappings_OrganizationDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "OrganizationDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncidenceTypeDepartmentMappings_IncidenceTypes_IncidenceTypeId",
                        column: x => x.IncidenceTypeId,
                        principalTable: "IncidenceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceTypeDepartmentMappings_DepartmentId",
                table: "IncidenceTypeDepartmentMappings",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceTypeDepartmentMappings_IncidenceTypeId",
                table: "IncidenceTypeDepartmentMappings",
                column: "IncidenceTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidenceTypeDepartmentMappings");
        }
    }
}
