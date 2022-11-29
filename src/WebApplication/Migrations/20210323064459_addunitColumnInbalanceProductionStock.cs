using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addunitColumnInbalanceProductionStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentBalanceProductionStocks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentBalanceProductionStocks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentBalanceProductionStocks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentBalanceProductionStocks");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentBalanceProductionStocks");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentBalanceProductionStocks");
        }
    }
}
