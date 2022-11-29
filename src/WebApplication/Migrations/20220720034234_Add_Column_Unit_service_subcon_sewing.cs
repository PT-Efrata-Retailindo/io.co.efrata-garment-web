using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Column_Unit_service_subcon_sewing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentServiceSubconSewingItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentServiceSubconSewingItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentServiceSubconSewingItems");
        }
    }
}
