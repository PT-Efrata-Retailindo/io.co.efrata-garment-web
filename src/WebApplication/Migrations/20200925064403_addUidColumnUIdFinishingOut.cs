using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addUidColumnUIdFinishingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentFinishingOuts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentFinishingOutItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentFinishingOuts");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentFinishingOutItems");
        }
    }
}
