using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class update_SampleRequestSpecification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UomId",
                table: "GarmentSampleRequestSpecifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UomUnit",
                table: "GarmentSampleRequestSpecifications",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UomId",
                table: "GarmentSampleRequestSpecifications");

            migrationBuilder.DropColumn(
                name: "UomUnit",
                table: "GarmentSampleRequestSpecifications");
        }
    }
}
