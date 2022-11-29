using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class add_buyer_gPreparings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "GarmentPreparings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "GarmentPreparings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "GarmentPreparings",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ROSource",
                table: "GarmentPreparingItems",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "GarmentPreparings");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "GarmentPreparings");

            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "GarmentPreparings");

            migrationBuilder.DropColumn(
                name: "ROSource",
                table: "GarmentPreparingItems");
        }
    }
}
