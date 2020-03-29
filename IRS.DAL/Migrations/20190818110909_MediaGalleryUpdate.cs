using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class MediaGalleryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.RenameColumn(
                name: "MaxFileSize",
                table: "SystemSettings",
                newName: "MaxVideoFileSize");

            migrationBuilder.RenameColumn(
                name: "AcceptedFileTypes",
                table: "SystemSettings",
                newName: "AcceptedVideoFileTypes");

            migrationBuilder.RenameColumn(
                name: "MaxFileSize",
                table: "Organizations",
                newName: "MaxVideoFileSize");

            migrationBuilder.RenameColumn(
                name: "AcceptedFileTypes",
                table: "Organizations",
                newName: "AcceptedVideoFileTypes");

            migrationBuilder.AddColumn<string>(
                name: "AcceptedImageFileTypes",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxImageFileSize",
                table: "SystemSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptedImageFileTypes",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxImageFileSize",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    RemoteUri = table.Column<string>(maxLength: 512, nullable: true),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    DateEdited = table.Column<DateTime>(nullable: true),
                    IncidenceId = table.Column<Guid>(nullable: true),
                    FileUploadChannel = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsVideo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gallery_Incidences_IncidenceId",
                        column: x => x.IncidenceId,
                        principalTable: "Incidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_IncidenceId",
                table: "Gallery",
                column: "IncidenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropColumn(
                name: "AcceptedImageFileTypes",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "MaxImageFileSize",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "AcceptedImageFileTypes",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "MaxImageFileSize",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "MaxVideoFileSize",
                table: "SystemSettings",
                newName: "MaxFileSize");

            migrationBuilder.RenameColumn(
                name: "AcceptedVideoFileTypes",
                table: "SystemSettings",
                newName: "AcceptedFileTypes");

            migrationBuilder.RenameColumn(
                name: "MaxVideoFileSize",
                table: "Organizations",
                newName: "MaxFileSize");

            migrationBuilder.RenameColumn(
                name: "AcceptedVideoFileTypes",
                table: "Organizations",
                newName: "AcceptedFileTypes");

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateEdited = table.Column<DateTime>(nullable: true),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    FileUploadChannel = table.Column<int>(nullable: true),
                    IncidenceId = table.Column<Guid>(nullable: true),
                    IsVideo = table.Column<bool>(nullable: false),
                    RemoteUri = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Incidences_IncidenceId",
                        column: x => x.IncidenceId,
                        principalTable: "Incidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    IncidenceId = table.Column<Guid>(nullable: true),
                    Url = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Incidences_IncidenceId",
                        column: x => x.IncidenceId,
                        principalTable: "Incidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IncidenceId",
                table: "Photos",
                column: "IncidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_IncidenceId",
                table: "Videos",
                column: "IncidenceId");
        }
    }
}
