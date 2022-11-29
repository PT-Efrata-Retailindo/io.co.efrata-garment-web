using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Field_GarmentServiceSubconSewing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentServiceSubconSewings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentServiceSubconSewings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentServiceSubconSewings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
