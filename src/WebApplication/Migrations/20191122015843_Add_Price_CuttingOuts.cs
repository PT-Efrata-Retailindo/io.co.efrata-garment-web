using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Price_CuttingOuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndirectPrice",
                table: "GarmentCuttingOutDetails");

            migrationBuilder.DropColumn(
                name: "OTL1",
                table: "GarmentCuttingOutDetails");

            migrationBuilder.RenameColumn(
                name: "OTL2",
                table: "GarmentCuttingOutDetails",
                newName: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "GarmentCuttingOutDetails",
                newName: "OTL2");

            migrationBuilder.AddColumn<double>(
                name: "IndirectPrice",
                table: "GarmentCuttingOutDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL1",
                table: "GarmentCuttingOutDetails",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
