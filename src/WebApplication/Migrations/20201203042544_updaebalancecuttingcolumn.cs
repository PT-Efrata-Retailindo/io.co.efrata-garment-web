using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class updaebalancecuttingcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentBalanceCuttings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentBalanceCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentBalanceCuttings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentBalanceCuttings");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentBalanceCuttings");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentBalanceCuttings");
        }
    }
}
