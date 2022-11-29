using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_Column_in_GarmentSewingDO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SewingOutDate",
                table: "GarmentSewingDOs",
                newName: "SewingDODate");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "GarmentSewingDOItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GarmentSewingDOItems");

            migrationBuilder.RenameColumn(
                name: "SewingDODate",
                table: "GarmentSewingDOs",
                newName: "SewingOutDate");
        }
    }
}
