using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_FinishingIns_add_DO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DOId",
                table: "GarmentFinishingIns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DONo",
                table: "GarmentFinishingIns",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOId",
                table: "GarmentFinishingIns");

            migrationBuilder.DropColumn(
                name: "DONo",
                table: "GarmentFinishingIns");
        }
    }
}
