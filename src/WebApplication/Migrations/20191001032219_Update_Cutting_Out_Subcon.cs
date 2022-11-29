using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_Cutting_Out_Subcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPOId",
                table: "GarmentCuttingOutItems");

            migrationBuilder.DropColumn(
                name: "EPOItemId",
                table: "GarmentCuttingOutItems");

            migrationBuilder.DropColumn(
                name: "POSerialNumber",
                table: "GarmentCuttingOutItems");

            migrationBuilder.AddColumn<long>(
                name: "EPOId",
                table: "GarmentCuttingOuts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EPOItemId",
                table: "GarmentCuttingOuts",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "POSerialNumber",
                table: "GarmentCuttingOuts",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPOId",
                table: "GarmentCuttingOuts");

            migrationBuilder.DropColumn(
                name: "EPOItemId",
                table: "GarmentCuttingOuts");

            migrationBuilder.DropColumn(
                name: "POSerialNumber",
                table: "GarmentCuttingOuts");

            migrationBuilder.AddColumn<long>(
                name: "EPOId",
                table: "GarmentCuttingOutItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "EPOItemId",
                table: "GarmentCuttingOutItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "POSerialNumber",
                table: "GarmentCuttingOutItems",
                maxLength: 100,
                nullable: true);
        }
    }
}
