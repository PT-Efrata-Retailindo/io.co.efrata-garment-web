using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addUidinCUttingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentCuttingOuts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentCuttingOutItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentCuttingOutDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentCuttingOuts");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentCuttingOutItems");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentCuttingOutDetails");
        }
    }
}
