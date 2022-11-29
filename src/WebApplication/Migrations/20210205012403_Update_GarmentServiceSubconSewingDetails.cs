using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_GarmentServiceSubconSewingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentServiceSubconSewingItems",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentServiceSubconSewingItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentServiceSubconSewingItems",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentServiceSubconSewingItems");

            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentServiceSubconSewings",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentServiceSubconSewings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentServiceSubconSewings",
                maxLength: 100,
                nullable: true);
        }
    }
}
