using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Subcon_Cutting_Out : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "GarmentCuttingOutDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "GarmentCuttingOutDetails");
        }
    }
}
