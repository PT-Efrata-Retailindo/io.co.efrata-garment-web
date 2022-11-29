using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addcolomnuomqtypackingsubconservicecutting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtyPacking",
                table: "GarmentServiceSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UomId",
                table: "GarmentServiceSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentServiceSubconCuttings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyPacking",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentServiceSubconCuttings");
        }
    }
}
