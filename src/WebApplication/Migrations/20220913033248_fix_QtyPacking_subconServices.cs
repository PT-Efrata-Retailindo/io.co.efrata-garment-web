using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class fix_QtyPacking_subconServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyPacking",
                table: "GarmentServiceSubconShrinkagePanels");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentServiceSubconShrinkagePanels");

            migrationBuilder.DropColumn(
                name: "QtyPacking",
                table: "GarmentServiceSubconFabricWashes");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentServiceSubconFabricWashes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtyPacking",
                table: "GarmentServiceSubconShrinkagePanels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentServiceSubconShrinkagePanels",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyPacking",
                table: "GarmentServiceSubconFabricWashes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentServiceSubconFabricWashes",
                nullable: true);
        }
    }
}
