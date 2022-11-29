using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_SewingOutItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "GarmentSewingOuts");

            migrationBuilder.AddColumn<double>(
                name: "RemainingQuantity",
                table: "GarmentSewingOutItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingQuantity",
                table: "GarmentSewingOutItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "GarmentSewingOuts",
                nullable: false,
                defaultValue: false);
        }
    }
}
