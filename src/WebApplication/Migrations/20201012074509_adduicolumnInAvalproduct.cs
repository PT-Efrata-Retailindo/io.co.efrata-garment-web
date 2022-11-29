using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class adduicolumnInAvalproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentAvalProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentAvalProductItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentAvalProducts");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentAvalProductItems");
        }
    }
}
