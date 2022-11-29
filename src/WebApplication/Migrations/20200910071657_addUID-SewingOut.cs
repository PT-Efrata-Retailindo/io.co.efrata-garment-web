using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addUIDSewingOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentSewingOuts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentSewingOutItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentSewingOuts");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentSewingOutItems");
        }
    }
}
