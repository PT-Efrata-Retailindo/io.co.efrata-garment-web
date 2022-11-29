using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Add_Field_GarmentServiceSubconCutting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentServiceSubconCuttings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentServiceSubconCuttings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentServiceSubconCuttings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentServiceSubconCuttings");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentServiceSubconCuttings");
        }
    }
}
