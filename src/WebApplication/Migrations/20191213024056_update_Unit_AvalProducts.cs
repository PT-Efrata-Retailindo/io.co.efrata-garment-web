using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_Unit_AvalProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentAvalProducts",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "GarmentAvalProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentAvalProducts",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentAvalProducts");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "GarmentAvalProducts");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentAvalProducts");
        }
    }
}
