using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Price_SewingOutItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicPrice",
                table: "GarmentSewingOutDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "GarmentSewingOutDetails");

            migrationBuilder.AddColumn<double>(
                name: "BasicPrice",
                table: "GarmentSewingOutItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "GarmentSewingOutItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicPrice",
                table: "GarmentSewingOutItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "GarmentSewingOutItems");

            migrationBuilder.AddColumn<double>(
                name: "BasicPrice",
                table: "GarmentSewingOutDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "GarmentSewingOutDetails",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
