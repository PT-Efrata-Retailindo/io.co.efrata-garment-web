using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Field_GarmentSubconContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentSubconContracts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentSubconContracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentSubconContracts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentSubconContracts");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentSubconContracts");
        }
    }
}
