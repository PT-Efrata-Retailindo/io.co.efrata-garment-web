using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class fix_add_QtyPacking_subconSewing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtyPacking",
                table: "GarmentServiceSubconSewings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentServiceSubconSewings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyPacking",
                table: "GarmentServiceSubconSewings");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentServiceSubconSewings");
        }
    }
}
