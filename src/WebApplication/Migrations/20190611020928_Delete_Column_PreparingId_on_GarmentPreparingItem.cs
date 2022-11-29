using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Delete_Column_PreparingId_on_GarmentPreparingItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreparingId",
                table: "GarmentPreparingItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreparingId",
                table: "GarmentPreparingItems",
                nullable: false,
                defaultValue: 0);
        }
    }
}
