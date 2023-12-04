using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addcolumnforrackingdeliveryReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "GarmentSampleDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Box",
                table: "GarmentSampleDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "GarmentSampleDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "GarmentSampleDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rack",
                table: "GarmentSampleDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Box",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rack",
                table: "GarmentDeliveryReturnItems",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "GarmentSampleDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Box",
                table: "GarmentSampleDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Colour",
                table: "GarmentSampleDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "GarmentSampleDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Rack",
                table: "GarmentSampleDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "GarmentDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Box",
                table: "GarmentDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Colour",
                table: "GarmentDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "GarmentDeliveryReturnItems");

            migrationBuilder.DropColumn(
                name: "Rack",
                table: "GarmentDeliveryReturnItems");
        }
    }
}
