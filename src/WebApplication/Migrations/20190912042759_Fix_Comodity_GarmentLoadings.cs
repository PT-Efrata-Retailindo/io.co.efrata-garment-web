using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class Fix_Comodity_GarmentLoadings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comodity",
                table: "GarmentLoadings",
                newName: "ComodityName");

            migrationBuilder.AddColumn<string>(
                name: "ComodityCode",
                table: "GarmentLoadings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComodityId",
                table: "GarmentLoadings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComodityCode",
                table: "GarmentLoadings");

            migrationBuilder.DropColumn(
                name: "ComodityId",
                table: "GarmentLoadings");

            migrationBuilder.RenameColumn(
                name: "ComodityName",
                table: "GarmentLoadings",
                newName: "Comodity");
        }
    }
}
