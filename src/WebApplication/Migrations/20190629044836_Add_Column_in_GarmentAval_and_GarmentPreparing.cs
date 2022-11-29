using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Column_in_GarmentAval_and_GarmentPreparing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCode",
                table: "GarmentPreparings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                table: "GarmentPreparings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentPreparingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentPreparingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentPreparingItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentAvalProductItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "GarmentAvalProductItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentAvalProductItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitCode",
                table: "GarmentPreparings");

            migrationBuilder.DropColumn(
                name: "UnitName",
                table: "GarmentPreparings");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentPreparingItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentPreparingItems");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentPreparingItems");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentAvalProductItems");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "GarmentAvalProductItems");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentAvalProductItems");
        }
    }
}
