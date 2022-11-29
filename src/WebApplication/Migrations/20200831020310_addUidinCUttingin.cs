using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addUidinCUttingin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentCuttingIns",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentCuttingInItems",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentCuttingInDetails",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentCuttingIns");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentCuttingInItems");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentCuttingInDetails");
        }
    }
}
