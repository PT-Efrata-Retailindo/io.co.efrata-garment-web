using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class addUIDSewingIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentSewingIns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UId",
                table: "GarmentSewingInItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentSewingIns");

            migrationBuilder.DropColumn(
                name: "UId",
                table: "GarmentSewingInItems");
        }
    }
}
