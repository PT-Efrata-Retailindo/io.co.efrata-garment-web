using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addColumnInExpenditureGoodExpenditureGoodReturnadjustmentfinishedGoodStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentFinishedGoodStocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentFinishedGoodStockHistories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentExpenditureGoods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentExpenditureGoodReturns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentExpenditureGoodReturnItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentExpenditureGoodItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentAdjustments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentAdjustmentItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentFinishedGoodStocks");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentFinishedGoodStockHistories");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentExpenditureGoods");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentExpenditureGoodReturns");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentExpenditureGoodReturnItems");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentExpenditureGoodItems");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentAdjustments");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentAdjustmentItems");
        }
    }
}
