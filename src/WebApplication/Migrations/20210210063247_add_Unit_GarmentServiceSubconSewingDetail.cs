using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_Unit_GarmentServiceSubconSewingDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentServiceSubconSewingDetails",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentServiceSubconSewingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentServiceSubconSewingDetails",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentServiceSubconSewingDetails");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentServiceSubconSewingDetails");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentServiceSubconSewingDetails");

            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentServiceSubconSewings",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentServiceSubconSewings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentServiceSubconSewings",
                maxLength: 100,
                nullable: true);
        }
    }
}
