using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Description_SampleStocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GarmentSampleStocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GarmentSampleStockHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "GarmentSampleStocks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GarmentSampleStockHistories");
        }
    }
}
