using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addUIdinAvalCOmponentItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "GarmentBalanceCuttings");

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentAvalComponentItems",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentAvalComponentItems");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "GarmentBalanceCuttings",
                maxLength: 50,
                nullable: true);
        }
    }
}
