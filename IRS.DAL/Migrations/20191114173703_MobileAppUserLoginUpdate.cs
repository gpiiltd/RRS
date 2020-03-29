using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class MobileAppUserLoginUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MobileAppLoginPattern",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.Sql("update AspNetUsers set IsActive = 1");
            migrationBuilder.Sql("update AspNetUsers set MobileAppLoginPattern = ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MobileAppLoginPattern",
                table: "AspNetUsers");
        }
    }
}
