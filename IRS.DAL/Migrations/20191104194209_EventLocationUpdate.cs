using Microsoft.EntityFrameworkCore.Migrations;

namespace IRS.DAL.Migrations
{
    public partial class EventLocationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRectified",
                table: "Hazards");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Incidences",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Hazards",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReportedIncidenceLongitude",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ReportedIncidenceLatitude",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Hazards",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReportedLongitude",
                table: "Hazards",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ReportedLatitude",
                table: "Hazards",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Incidences",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Hazards",
                newName: "address");

            migrationBuilder.AlterColumn<double>(
                name: "ReportedIncidenceLongitude",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReportedIncidenceLatitude",
                table: "Incidences",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Incidences",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<double>(
                name: "ReportedLongitude",
                table: "Hazards",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReportedLatitude",
                table: "Hazards",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Hazards",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "IsRectified",
                table: "Hazards",
                nullable: false,
                defaultValue: false);
        }
    }
}
