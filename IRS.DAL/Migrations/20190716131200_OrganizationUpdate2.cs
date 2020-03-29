using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class OrganizationUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update Organizations set MaxFileSize=0 where MaxFileSize<0 or MaxFileSize is NULL");
            migrationBuilder.AlterColumn<int>(
                name: "MaxFileSize",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaxFileSize",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
