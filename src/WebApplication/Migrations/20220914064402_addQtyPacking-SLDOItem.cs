using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addQtyPackingSLDOItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtyPacking",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomSatuanUnit",
                table: "GarmentSubconDeliveryLetterOutItems",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyPacking",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropColumn(
                name: "UomSatuanUnit",
                table: "GarmentSubconDeliveryLetterOutItems");
        }
    }
}
