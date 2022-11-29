using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Update_garmentSampleRequests_Add_Section : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SectionCode",
                table: "GarmentSampleRequests",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "GarmentSampleRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionCode",
                table: "GarmentSampleRequests");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "GarmentSampleRequests");
        }
    }
}
