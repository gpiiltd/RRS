using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class HazardFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HazardDefaultDepartmentId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HazardId",
                table: "Gallery",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Hazards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    Comment = table.Column<string>(maxLength: 1024, nullable: true),
                    RouteCause = table.Column<string>(maxLength: 255, nullable: true),
                    AreaId = table.Column<Guid>(nullable: true),
                    CityId = table.Column<Guid>(nullable: true),
                    StateId = table.Column<Guid>(nullable: true),
                    CountryId = table.Column<Guid>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    ReportedLatitude = table.Column<double>(nullable: false),
                    ReportedLongitude = table.Column<double>(nullable: false),
                    Suggestion = table.Column<string>(maxLength: 255, nullable: true),
                    AssignerId = table.Column<Guid>(nullable: true),
                    AssigneeId = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    DateEdited = table.Column<DateTime>(nullable: true),
                    EditedByUserId = table.Column<Guid>(nullable: true),
                    AssignedOrganizationId = table.Column<Guid>(nullable: true),
                    AssignedDepartmentId = table.Column<Guid>(nullable: true),
                    ReporterName = table.Column<string>(maxLength: 50, nullable: true),
                    ReporterEmail = table.Column<string>(maxLength: 50, nullable: true),
                    ReporterFirstResponderAction = table.Column<string>(maxLength: 1024, nullable: true),
                    ReporterFeedbackRating = table.Column<int>(nullable: true),
                    ManagerFeedbackRating = table.Column<int>(nullable: true),
                    ReporterDepartmentId = table.Column<Guid>(nullable: true),
                    IncidenceStatusId = table.Column<Guid>(nullable: true),
                    ResolutionDate = table.Column<DateTime>(nullable: true),
                    Protected = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    IsRectified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hazards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hazards_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_OrganizationDepartments_AssignedDepartmentId",
                        column: x => x.AssignedDepartmentId,
                        principalTable: "OrganizationDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_Organizations_AssignedOrganizationId",
                        column: x => x.AssignedOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_AspNetUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_AspNetUsers_AssignerId",
                        column: x => x.AssignerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hazards_AspNetUsers_EditedByUserId",
                        column: x => x.EditedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_IncidenceStatuses_IncidenceStatusId",
                        column: x => x.IncidenceStatusId,
                        principalTable: "IncidenceStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hazards_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_HazardId",
                table: "Gallery",
                column: "HazardId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_AreaId",
                table: "Hazards",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_AssignedDepartmentId",
                table: "Hazards",
                column: "AssignedDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_AssignedOrganizationId",
                table: "Hazards",
                column: "AssignedOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_AssigneeId",
                table: "Hazards",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_AssignerId",
                table: "Hazards",
                column: "AssignerId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_CityId",
                table: "Hazards",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_CountryId",
                table: "Hazards",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_CreatedByUserId",
                table: "Hazards",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_EditedByUserId",
                table: "Hazards",
                column: "EditedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_IncidenceStatusId",
                table: "Hazards",
                column: "IncidenceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_OrganizationId",
                table: "Hazards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_StateId",
                table: "Hazards",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gallery_Hazards_HazardId",
                table: "Gallery",
                column: "HazardId",
                principalTable: "Hazards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gallery_Hazards_HazardId",
                table: "Gallery");

            migrationBuilder.DropTable(
                name: "Hazards");

            migrationBuilder.DropIndex(
                name: "IX_Gallery_HazardId",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "HazardDefaultDepartmentId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "HazardId",
                table: "Gallery");
        }
    }
}
