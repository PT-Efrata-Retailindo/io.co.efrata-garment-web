using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addQtyPacking_SLDO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtyPacking",
                table: "GarmentSubconDeliveryLetterOuts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentSubconDeliveryLetterOuts",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyPacking",
                table: "GarmentSubconDeliveryLetterOuts");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentSubconDeliveryLetterOuts");
        }
    }
}
