using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class MediaUserProfilePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Gallery",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_UserId",
                table: "Gallery",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gallery_AspNetUsers_UserId",
                table: "Gallery",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gallery_AspNetUsers_UserId",
                table: "Gallery");

            migrationBuilder.DropIndex(
                name: "IX_Gallery_UserId",
                table: "Gallery");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Gallery");
        }
    }
}
