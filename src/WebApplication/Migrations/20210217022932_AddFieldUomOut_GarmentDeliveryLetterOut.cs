using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class AddFieldUomOut_GarmentDeliveryLetterOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UomOutId",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomOutUnit",
                table: "GarmentSubconDeliveryLetterOutItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UomOutId",
                table: "GarmentSubconDeliveryLetterOutItems");

            migrationBuilder.DropColumn(
                name: "UomOutUnit",
                table: "GarmentSubconDeliveryLetterOutItems");
        }
    }
}
